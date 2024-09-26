using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LMS.Domain.DTOs.Book
{
    public class BookDTO
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        [StringLength(100)]
        public string Author { get; set; }
        public string? Publisher { get; set; }
        [JsonIgnore]
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
        public string ImageURL { get; set; }
    }
}
