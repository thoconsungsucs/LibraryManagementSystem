namespace LMS.Domain.Ultilities
{
    public class LoanFilter
    {
        public int? MemberId { get; set; }
        public string? BookTitle { get; set; }
        public DateOnly LoanDate { get; set; }
        public DateOnly ReturnDate { get; set; }
        public string? Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }
}
