using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.DTOs.Loan
{
    public class LoanDTOForPut
    {
        public int Id { get; set; }
        public DateOnly LoanDate { get; set; }
        [Required]
        [Range(1, 30)]
        public int LoanDuration { get; set; }
    }
}
