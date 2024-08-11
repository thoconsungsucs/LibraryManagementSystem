using LMS.Domain.DTOs.Member;
using LMS.Domain.IRepository;
using LMS.Domain.IService;
using LMS.Domain.Mappers;

namespace LMS.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _studentRepository;
        public MemberService(IMemberRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public async Task<List<MemberDTO>> GetAllStudents()
        {
            var students = await _studentRepository.GetAllMembers();
            return students.Select(s => s.ToMemberDTO()).ToList();
        }
    }
}
