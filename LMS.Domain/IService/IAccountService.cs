using FluentValidation.Results;
using LMS.Domain.DTOs.Account;
using LMS.Domain.DTOs.Librarian;
using LMS.Domain.DTOs.Member;

namespace LMS.Domain.IService
{
    public interface IAccountRepository
    {
        Task<ValidationResult> RegisterMemberAsync(MemberRegisterDTO studentRegisterDTO);
        Task<ValidationResult> RegisterLibrarianAsync(LibrarianRegisterDTO librarianRegisterDTO);
        Task<NewUser> LoginAsync(LoginDTO loginDTO);
    }
}
