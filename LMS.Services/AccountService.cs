using LMS.Domain.DTOs.Account;
using LMS.Domain.DTOs.Student;
using LMS.Domain.IService;
using LMS.Domain.Mappers;
using Microsoft.AspNetCore.Identity;

namespace LMS.Services
{
    public class AccountService : IAccountService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountService(ITokenService tokenService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
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
                    Token = _tokenService.GenerateToken(user.UserName, roles.ToList())
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<NewUser> RegisterAsync(StudentRegisterDTO studentRegisterDTO)
        {
            var student = studentRegisterDTO.ToStudent();
            try
            {

                var result = await _userManager.CreateAsync(student, studentRegisterDTO.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join("\n", result.Errors.Select(e => e.Description));
                    throw new Exception($"User registration failed: \n{errors}");
                }
                var roleResult = await _userManager.AddToRoleAsync(student, "Student");
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join("\n", result.Errors.Select(e => e.Description));
                    throw new Exception($"User registration failed: {errors}");
                }
                return new NewUser
                {
                    UserName = student.UserName,
                    Token = _tokenService.GenerateToken(student.UserName)
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
