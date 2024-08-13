namespace LMS.Domain.DTOs.Loan
{
    public class LoanDTOForPut
    {
        int Id { get; set; }
        public DateOnly ReturnDate { get; set; }
    }
}
