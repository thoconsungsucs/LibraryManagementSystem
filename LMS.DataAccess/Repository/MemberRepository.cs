using LMS.DataAccess.Data;
using LMS.Domain.IRepository;
using LMS.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.DataAccess.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly ApplicationDbContext _context;
        public MemberRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Member>> GetAllMembers()
        {
            return await _context.Members.Select(s => new Member
            {
                Id = s.Id,
                Email = s.Email,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Address = s.Address,
                PhoneNumber = s.PhoneNumber
            }).ToListAsync();
        }
    }
}
