using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.DTOs.Account
{
    public class LoginDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
