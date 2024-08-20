namespace LMS.Domain.DTOs.Loan
{
    public class LoanDTOForPost : LoanToDb
    {
        public int MemberId { get; set; }
        public int BookId { get; set; }
    }
}
