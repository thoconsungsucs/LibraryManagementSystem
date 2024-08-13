using LMS.Domain.DTOs.Account;
using LMS.Domain.DTOs.Librarian;
using LMS.Domain.DTOs.Member;
using LMS.Domain.IService;
using LMS.Domain.Mappers;
using LMS.Domain.Ultilities;
using Microsoft.AspNetCore.Identity;

namespace LMS.Services
{
    public class AccountService : IAccountRepository
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly SignInManager<IdentityUser<int>> _signInManager;

        public AccountService(ITokenService tokenService, UserManager<IdentityUser<int>> userManager, SignInManager<IdentityUser<int>> signInManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<NewUser> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(loginDTO.UserName);
                if (user == null)
                {
                    throw new Exception("User not found or password is invalid.");
                }
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
                if (!result.Succeeded)
                {
                    throw new Exception("User not found or password is invalid.");
                }

                var roles = await _userManager.GetRolesAsync(user);
                return new NewUser
                {
                    UserName = user.UserName,
                    Token = _tokenService.GenerateToken(user.Id, user.UserName, roles.ToList())
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<NewUser> RegisterMemberAsync(MemberRegisterDTO memberRegisterDTO)
        {
            var member = memberRegisterDTO.ToMember();
            try
            {
                var newUser = await RegisterAsyncHelper(member, memberRegisterDTO.Password);
                return newUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<NewUser> RegisterLibrarianAsync(LibrarianRegisterDTO librarianRegisterDTO)
        {
            var librarian = librarianRegisterDTO.ToLibrarian();
            try
            {
                var newUser = await RegisterAsyncHelper(librarian, librarianRegisterDTO.Password);
                return newUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<NewUser> RegisterAsyncHelper(IdentityUser<int> user, string password)
        {
            try
            {
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    var errors = string.Join("\n", result.Errors.Select(e => e.Description));
                    throw new Exception($"User registration failed: \n{errors}");
                }
                var roleResult = await _userManager.AddToRoleAsync(user, SD.Role_Member);
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join("\n", result.Errors.Select(e => e.Description));
                    throw new Exception($"User registration failed: {errors}");
                }
                return new NewUser
                {
                    UserName = user.UserName,
                    Token = _tokenService.GenerateToken(user.Id, user.UserName)
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
