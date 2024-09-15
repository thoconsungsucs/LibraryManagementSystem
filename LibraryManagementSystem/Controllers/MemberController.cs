using LMS.Domain.DTOs.Member;
using LMS.Domain.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _MemberService;
        public MemberController(IMemberService MemberService)
        {
            _MemberService = MemberService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetAllMembers()
        {
            try
            {
                var Members = await _MemberService.GetAllMembers();
                return Ok(Members);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDTO>> GetMember(int id)
        {
            try
            {
                var Member = await _MemberService.GetMember(id);
                return Ok(Member);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<MemberDTO>> UpdateMember(MemberDTO Member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedMember = await _MemberService.UpdateMember(Member);
                return Ok(updatedMember);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Member")]
        public async Task<ActionResult<MemberDTO>> DeleteMember(int id)
        {
            try
            {
                var deletedMember = await _MemberService.DeleteMember(id);
                return Ok(deletedMember);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
