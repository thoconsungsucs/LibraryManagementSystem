using LMS.Domain.DTOs.Book;
using LMS.Domain.Models;

namespace LMS.Domain.Mappers
{
    public static class BookMappers
    {
        public static Book ToBook(this BookDTO bookDTO)
        {
            return new Book
            {
                Title = bookDTO.Title,
                Author = bookDTO.Author,
                Publisher = bookDTO.Publisher,
                PublishedDate = bookDTO.PublishedDate,
                ISBN = bookDTO.ISBN,
                Category = bookDTO.Category,
                Pages = bookDTO.Pages,
                Description = bookDTO.Description,
                Quantity = bookDTO.Quantity,
                Available = bookDTO.Quantity,
                ImageURL = bookDTO.ImageURL
            };
        }
    }
}
