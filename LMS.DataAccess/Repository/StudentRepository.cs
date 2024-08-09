using LMS.DataAccess.Data;
using LMS.Domain.IRepository;
using LMS.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.DataAccess.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;
        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Student>> GetAllStudents()
        {
            return await _context.Students.Select(s => new Student
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
