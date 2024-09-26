using LMS.Domain.DTOs.Account;
using LMS.Domain.DTOs.Librarian;
using LMS.Domain.DTOs.Member;
using LMS.Domain.Exceptions;

namespace LMS.Domain.IService
{
    public interface IAccountService
    {
        Task<Result<NewUser>> RegisterMemberAsync(MemberRegisterDTO studentRegisterDTO);
        Task<Result<NewUser>> RegisterLibrarianAsync(LibrarianRegisterDTO librarianRegisterDTO);
        Task<Result<NewUser>> LoginAsync(LoginDTO loginDTO);
    }
}
