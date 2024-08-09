using LMS.Domain.Models;

namespace LMS.Domain.IRepository
{
    public interface IStudentRepository
    {
        public Task<List<Student>> GetAllStudents();
    }
}
