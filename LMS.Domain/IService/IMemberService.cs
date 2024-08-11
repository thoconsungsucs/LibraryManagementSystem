using LMS.Domain.DTOs.Member;
namespace LMS.Domain.IService
{
    public interface IMemberService
    {
        public Task<List<MemberDTO>> GetAllStudents();
    }
}
