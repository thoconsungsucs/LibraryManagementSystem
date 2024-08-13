namespace LMS.Domain.DTOs.Loan
{
    public class LoanDTOForPut
    {
        public int Id { get; set; }
        public DateOnly LoanDate { get; set; } 
        public int LoanDuration { get; set; }
    }
}
