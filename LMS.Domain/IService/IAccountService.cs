using LMS.Domain.DTOs.Account;
using LMS.Domain.DTOs.Librarian;
using LMS.Domain.DTOs.Member;

namespace LMS.Domain.IService
{
    public interface IAccountRepository
    {
        Task<NewUser> RegisterMemberAsync(MemberRegisterDTO studentRegisterDTO);
        Task<NewUser> RegisterLibrarianAsync(LibrarianRegisterDTO librarianRegisterDTO);
        Task<NewUser> LoginAsync(LoginDTO loginDTO);
    }
}
