using LMS.Domain.DTOs.Book;
using LMS.Domain.Models;
using LMS.Domain.Ultilities;

namespace LMS.Domain.IService
{
    public interface IBookService
    {
        Task<List<Book>> GetAllBooks(BookFilter filter);
        Task<Book> GetBook(int id);
        Task<Book> AddBook(BookDTO bookDTO);
        Task<Book> UpdateBook(int id, BookDTO bookDTO);
        Task<Book> DeleteBookAsync(int id);
        Task<int> GetTotalNumberOfBook();
    }
}
