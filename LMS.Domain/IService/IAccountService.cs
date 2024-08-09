using LMS.Domain.DTOs.Account;
using LMS.Domain.DTOs.Student;

namespace LMS.Domain.IService
{
    public interface IAccountService
    {
        Task<NewUser> RegisterAsync(StudentRegisterDTO studentRegisterDTO);
        Task<NewUser> LoginAsync(LoginDTO loginDTO);
    }
}
