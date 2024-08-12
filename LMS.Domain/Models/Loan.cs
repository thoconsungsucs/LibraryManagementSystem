using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Domain.Models
{
    public class Loan
    {
        public int Id { get; set; }
        [Required]
        public string MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Member Member { get; set; }
        [Required]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime LoanDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? ActualReturnDate { get; set; }
    }
}
