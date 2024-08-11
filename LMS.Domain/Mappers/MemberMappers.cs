using LMS.Domain.DTOs.Member;
using LMS.Domain.Models;
namespace LMS.Domain.Mappers
{
    public static class MemberMappers
    {
        public static Member ToMember(this MemberRegisterDTO MemberRegisterDTO)
        {
            return new Member
            {
                StudentId = MemberRegisterDTO.StudentId,
                UserName = MemberRegisterDTO.UserName,
                FirstName = MemberRegisterDTO.FirstName,
                LastName = MemberRegisterDTO.LastName,
                Address = MemberRegisterDTO.Address,
                Email = MemberRegisterDTO.Email,
                PhoneNumber = MemberRegisterDTO.PhoneNumber
            };
        }

        public static MemberDTO ToMemberDTO(this Member Member)
        {
            return new MemberDTO
            {
                Id = Member.Id,
                StudentId = Member.StudentId,
                Email = Member.Email,
                FirstName = Member.FirstName,
                LastName = Member.LastName,
                Address = Member.Address,
                PhoneNumber = Member.PhoneNumber
            };
        }
    }
}
