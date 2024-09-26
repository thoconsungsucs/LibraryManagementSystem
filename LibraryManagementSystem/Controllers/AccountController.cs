using LMS.Domain.DTOs.Account;
using LMS.Domain.DTOs.Librarian;
using LMS.Domain.DTOs.Member;
using LMS.Domain.Exceptions;
using LMS.Domain.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(MemberRegisterDTO studentRegisterDTO)
        {

            try
            {
                var newUser = await _accountService.RegisterMemberAsync(studentRegisterDTO);

                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("register-librarian")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterLibrarianAsync(LibrarianRegisterDTO librarianRegisterDTO)
        {

            try
            {
                var newUser = await _accountService.RegisterLibrarianAsync(librarianRegisterDTO);

                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                var tokenResult = await _accountService.LoginAsync(loginDTO);
                return tokenResult.IsSuccess ? Ok(tokenResult.Value) : Unauthorized(ApiResult.ToProblemDetails(tokenResult));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
