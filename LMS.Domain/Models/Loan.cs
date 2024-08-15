using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Domain.Models
{
    public class Loan
    {
        public int Id { get; set; }
        [Required]
        public int MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Member Member { get; set; }
        [Required]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateOnly LoanDate { get; set; }
        [DataType(DataType.Date)]
        public DateOnly ReturnDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateOnly? ActualReturnDate { get; set; }
        public DateOnly? UpdateReturnDate { get; set; }
        public string Status { get; set; }
    }
}
