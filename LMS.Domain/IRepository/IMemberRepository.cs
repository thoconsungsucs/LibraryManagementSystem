using LMS.Domain.Models;

namespace LMS.Domain.IRepository
{
    public interface IMemberRepository
    {
        Task<List<Member>> GetAllMembers();
        Task<Member> GetMember(int? id);
        void UpdateMember(Member member);
        void DeleteMember(Member member);
        Task SaveAsync();
        Task<Member> GetMemberInformation(int memberId);
    }
}
