using LMS.Domain.Models;
using LMS.Domain.Ultilities;

namespace LMS.Domain.IService
{
    public interface IBookService
    {
        Task<List<Book>> GetAllBooks(BookFilter filter);
        Task<Book> GetBook(int id);
        Task<Book> UpdateBook(Book book);
        Task<Book> DeleteBookAsync(int id);
    }
}
