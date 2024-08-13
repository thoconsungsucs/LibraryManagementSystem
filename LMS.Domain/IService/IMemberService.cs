using LMS.Domain.DTOs.Member;
namespace LMS.Domain.IService
{
    public interface IMemberService
    {
        public Task<List<MemberDTO>> GetAllMembers();
        public Task<MemberDTO> GetMember(int id);
        public Task<MemberDTO> UpdateMember(MemberDTO member);
        public Task<MemberDTO> DeleteMember(int id);
    }
}
