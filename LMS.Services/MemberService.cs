using LMS.Domain.DTOs.Member;
using LMS.Domain.IRepository;
using LMS.Domain.IService;
using LMS.Domain.Mappers;

namespace LMS.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }
        public async Task<List<MemberDTO>> GetAllMembers()
        {
            var members = await _memberRepository.GetAllMembers();
            return members.Select(s => s.ToMemberDTO()).ToList();
        }

        public async Task<MemberDTO> GetMember(string id)
        {
            var member = await _memberRepository.GetMember(id);
            return member.ToMemberDTO();
        }

        public async Task<MemberDTO> UpdateMember(MemberDTO member)
        {

            var memberToUpdate = await _memberRepository.GetMember(member.Id);
            if (memberToUpdate == null)
            {
                throw new Exception("Member not found");
            }

            memberToUpdate.StudentId = member.StudentId;
            memberToUpdate.Email = member.Email;
            memberToUpdate.FirstName = member.FirstName;
            memberToUpdate.LastName = member.LastName;
            memberToUpdate.Address = member.Address;
            memberToUpdate.PhoneNumber = member.PhoneNumber;

            _memberRepository.UpdateMember(memberToUpdate);
            await _memberRepository.SaveAsync();
            return member;
        }

        public async Task<MemberDTO> DeleteMember(string id)
        {
            var member = await _memberRepository.GetMember(id);
            if (member == null)
            {
                throw new Exception("Member not found");
            }

            _memberRepository.DeleteMember(member);
            await _memberRepository.SaveAsync();
            return member.ToMemberDTO();
        }
    }
}
