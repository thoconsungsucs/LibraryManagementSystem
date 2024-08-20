namespace LMS.Domain.DTOs.Loan
{
    public class LoanToDb
    {
        public DateOnly LoanDate { get; set; }
        public int Duration { get; set; }
    }
}
