using LMS.Domain.DTOs.Book;
using LMS.Domain.Exceptions;
using LMS.Domain.Models;
using LMS.Domain.Ultilities;

namespace LMS.Domain.IService
{
    public interface IBookService
    {
        Task<(List<Book>, int)> GetBooksAndCountAsync(BookFilter filter);
        Task<Result<Book>> GetBook(int id);
        Task<Result<Book>> AddBook(BookDTO bookDTO);
        Task<Result<Book>> UpdateBook(int id, BookDTO bookDTO);
        Task<Result<Book>> DeleteBookAsync(int id);
    }
}
