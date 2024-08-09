using LMS.Domain.DTOs.Student;

namespace LMS.Domain.IService
{
    public interface IStudentService
    {
        public Task<List<StudentDTO>> GetAllStudents();
    }
}
