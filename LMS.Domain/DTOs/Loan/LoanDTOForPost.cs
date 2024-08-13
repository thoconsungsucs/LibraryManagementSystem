using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.DTOs.Loan
{
    public class LoanDTOForPost
    {
        public string MemberId { get; set; }
        public int BookId { get; set; }
        public DateOnly LoanDate { get; set; }
        [Range(1, 30)]
        public int LoanDuration { get; set; }
    }
}
