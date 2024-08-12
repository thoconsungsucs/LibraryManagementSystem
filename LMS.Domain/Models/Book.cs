using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        [StringLength(100)]
        public string Author { get; set; }
        public string? Publisher { get; set; }
        public DateOnly? PublishedDate { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public int Pages { get; set; }
        public string? Description { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Available { get; set; }
    }
}
