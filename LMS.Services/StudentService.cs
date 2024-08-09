using LMS.Domain.DTOs.Student;
using LMS.Domain.IRepository;
using LMS.Domain.IService;
using LMS.Domain.Mappers;

namespace LMS.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public async Task<List<StudentDTO>> GetAllStudents()
        {
            var students = await _studentRepository.GetAllStudents();
            return students.Select(s => s.ToStudentDTO()).ToList();
        }
    }
}
