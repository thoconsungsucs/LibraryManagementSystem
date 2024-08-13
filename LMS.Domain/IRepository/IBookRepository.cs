using LMS.Domain.Models;

namespace LMS.Domain.IRepository
{
    public interface IBookRepository
    {
        IQueryable<Book> GetAllBooks();
        Task<Book> GetBook(int id);
        void AddBook(Book book);
        void UpdateBook(Book book);
        void DeletBook(Book book);
        Task SaveAsync();
    }
}
