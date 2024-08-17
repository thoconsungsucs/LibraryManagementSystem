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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var loan = await _loanService.AddLoan(loanDTO, isLibrarian: true);
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
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
        [Route("confirm-loan")]
        public async Task<IActionResult> ConfirmLoan(LoanConfirmDTO loanConfirmDTO)
        {
            try
            {
                var loan = await _loanService.ConfirmLoan(loanConfirmDTO);
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
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

        /*[HttpPost("confirm-update/{id}")]
        public async Task<IActionResult> ConfirmUpdateLoan(LoanConfirmDTO loanConfirmDTO)
        {
            try
            {
                var loan = await _loanService.ConfirmUpdate(loanConfirmDTO.LoanId);
                return Ok(loan);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }*/

        [HttpPost("renew/{id}")]
        public async Task<IActionResult> RenewLoan(int id, [FromQuery] int days)
        {
            try
            {
                var loan = await _loanService.RenewLoan(id, days);
                return Ok(loan);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("confirm-renew")]
        public async Task<IActionResult> ConfirmRenewLoan(LoanConfirmDTO loanConfirmDTO)
        {
            try
            {
                var loan = await _loanService.ConfirmRenew(loanConfirmDTO);
                return Ok(loan);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("return/{id}")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            try
            {
                var loan = await _loanService.ReturnBook(id);
                return Ok(loan);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("confirm-return/{id}")]
        public async Task<IActionResult> ConfirmReturn(int id)
        {
            try
            {
                var loan = await _loanService.ConfirmReturn(id);
                return Ok(loan);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            try
            {
                var loan = await _loanService.DeleteLoan(id);
                return Ok(loan);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
