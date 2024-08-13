namespace LMS.Domain.DTOs.Loan
{
    public class LoanDTO
    {
        public int Id { get; set; }
        public string MemberId { get; set; }
        public string MemberName { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public DateOnly LoanDate { get; set; }
        public DateOnly ReturnDate { get; set; }
        public DateOnly? ActualReturnDate { get; set; }
        public string Status { get; set; }
    }
}
