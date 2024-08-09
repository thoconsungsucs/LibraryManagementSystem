using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.DTOs.Student
{
    public class StudentDTO
    {
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Length(1, 15, ErrorMessage = "Length is larger than 0 and shorter than 16")]
        public string FirstName { get; set; }
        [Required]
        [Length(1, 15, ErrorMessage = "Length is larger than 0 and shorter than 16")]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
