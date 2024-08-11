using LMS.Domain.Models;

namespace LMS.Domain.IRepository
{
    public interface IMemberRepository
    {
        public Task<List<Member>> GetAllMembers();
    }
}
