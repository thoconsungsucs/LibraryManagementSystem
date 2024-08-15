using LMS.Domain.DTOs.Loan;
using LMS.Domain.IService;
using LMS.Domain.Ultilities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/loan")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLoans([FromQuery] LoanFilter filter)
        {
            var loans = await _loanService.GetAllLoans(filter);
            return Ok(loans);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoan(int id)
        {
            var loan = await _loanService.GetLoan(id);
            return Ok(loan);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoan([FromBody] LoanDTOForPost loanDTO)
        {
            try
            {
                var loan = await _loanService.AddLoan(loanDTO);
                return Ok(loan);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("member")]
        public async Task<IActionResult> CreateLoanByMember([FromBody] LoanDTOForPost loanDTO)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            loanDTO.MemberId = userId;

            var canBorrow = await _loanService.CanBorrow(userId);
            if (!canBorrow)
            {
                return BadRequest("You can't borrow more books");
            }
            try
            {
                var loan = await _loanService.AddLoan(loanDTO);
                return Ok(loan);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("confirm/{id}")]
        public async Task<IActionResult> ConfirmLoan(int id)
        {
            try
            {
                var loan = await _loanService.ConfirmLoan(id);
                return Ok(loan);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLoan(int id, [FromBody] LoanDTOForPut loanDTO)
        {
            loanDTO.Id = id;
            try
            {
                var loan = await _loanService.UpdateLoan(loanDTO);
                return Ok(loan);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
