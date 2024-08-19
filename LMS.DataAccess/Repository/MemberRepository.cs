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
        public async Task<Member> GetMember(int? id)
        {
            return await _context.Members.FirstOrDefaultAsync(s => s.Id == id);
        }

        public void UpdateMember(Member member)
        {
            _context.Members.Update(member);
        }

        public void DeleteMember(Member member)
        {
            _context.Members.Remove(member);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Member> GetMemberInformation(int id)
        {
            return await _context.Members.Where(m => m.Id == id).Select(m => new Member
            {
                FirstName = m.FirstName,
                LastName = m.LastName,
                Email = m.Email,
            }).FirstOrDefaultAsync();
        }
    }
}
