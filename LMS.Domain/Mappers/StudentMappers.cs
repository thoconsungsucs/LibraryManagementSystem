using LMS.Domain.DTOs.Student;
using LMS.Domain.Models;

namespace LMS.Domain.Mappers
{
    public static class StudentMappers
    {
        public static Student ToStudent(this StudentRegisterDTO studentRegisterDTO)
        {
            return new Student
            {
                UserName = studentRegisterDTO.UserName,
                FirstName = studentRegisterDTO.FirstName,
                LastName = studentRegisterDTO.LastName,
                Address = studentRegisterDTO.Address,
                Email = studentRegisterDTO.Email,
                PhoneNumber = studentRegisterDTO.PhoneNumber
            };
        }

        public static StudentDTO ToStudentDTO(this Student student)
        {
            return new StudentDTO
            {
                Id = student.Id,
                Email = student.Email,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Address = student.Address,
                PhoneNumber = student.PhoneNumber
            };
        }
    }
}
