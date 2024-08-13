namespace LMS.Domain.Ultilities
{
    public class BookFilter
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Publisher { get; set; }
        public int[]? Years { get; set; }
        public string[]? Categories { get; set; }
        public int? Pages { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
