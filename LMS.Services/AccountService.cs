using FluentValidation;
using LMS.Domain.DTOs.Account;
using LMS.Domain.DTOs.Librarian;
using LMS.Domain.DTOs.Member;
using LMS.Domain.Exceptions;
using LMS.Domain.IService;
using LMS.Domain.Mappers;
using LMS.Domain.Ultilities;
using Microsoft.AspNetCore.Identity;

namespace LMS.Services
{
    public class AccountService : IAccountService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly SignInManager<IdentityUser<int>> _signInManager;
        private readonly IValidator<MemberRegisterDTO> _memberValidator;
        private readonly IValidator<LibrarianRegisterDTO> _librarianValidator;

        public AccountService(
            ITokenService tokenService,
            UserManager<IdentityUser<int>> userManager,
            SignInManager<IdentityUser<int>> signInManager,
            IValidator<MemberRegisterDTO> memberValidator,
            IValidator<LibrarianRegisterDTO> librarianValidator
            )
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
            _memberValidator = memberValidator;
            _librarianValidator = librarianValidator;
        }

        public async Task<Result<NewUser>> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(loginDTO.UserName);
                if (user == null)
                {
                    return UserError.InvalidCredentials();
                }
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
                if (!result.Succeeded)
                {
                    return UserError.InvalidCredentials();
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

        public async Task<Result<NewUser>> RegisterMemberAsync(MemberRegisterDTO memberRegisterDTO)
        {
            var validationResult = await _memberValidator.ValidateAsync(memberRegisterDTO);
            var member = memberRegisterDTO.ToMember();

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

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

        public async Task<Result<NewUser>> RegisterLibrarianAsync(LibrarianRegisterDTO librarianRegisterDTO)
        {
            var validationResult = await _librarianValidator.ValidateAsync(librarianRegisterDTO);
            var librarian = librarianRegisterDTO.ToLibrarian();

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            try
            {
                var newUser = await RegisterAsyncHelper(librarian, librarianRegisterDTO.Password, SD.Role_Librarian);
                return newUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<Result<NewUser>> RegisterAsyncHelper(IdentityUser<int> user, string password, string role = SD.Role_Member)
        {
            try
            {
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    var errors = string.Join("\n", result.Errors.Select(e => e.Description));
                    throw new Exception($"User registration failed: \n{errors}");
                }
                var roleResult = await _userManager.AddToRoleAsync(user, role);
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
