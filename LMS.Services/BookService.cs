using LMS.Domain.IRepository;
using LMS.Domain.IService;
using LMS.Domain.Models;
using LMS.Domain.Ultilities;
using Microsoft.EntityFrameworkCore;

namespace LMS.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public async Task<List<Book>> GetAllBooks(BookFilter filter)
        {
            var books = _bookRepository.GetAllBooks();

            if (filter.IsAvailable)
            {
                books = books.Where(x => x.Available > 0);
            }

            if (!string.IsNullOrEmpty(filter.Title))
            {
                books = books.Where(x => x.Title.Contains(filter.Title));
            }

            if (!string.IsNullOrEmpty(filter.Author))
            {
                books = books.Where(x => x.Author.Contains(filter.Author));
            }

            if (filter.Years != null)
            {
                books = books.Where(x => x.PublishedDate != null && filter.Years.Contains(x.PublishedDate.Value.Year));
            }

            if (filter.Pages != 0)
            {
                books = books.Where(x => x.Pages <= filter.Pages);
            }

            if (!string.IsNullOrEmpty(filter.Publisher))
            {
                books = books.Where(x => x.Publisher != null && x.Publisher.Contains(filter.Publisher));
            }

            if (filter.Categories != null)
            {
                books = books.Where(x => filter.Categories.Contains(x.Category));
            }
            return await books.ToListAsync();
        }

        public async Task<Book> GetBook(int id)
        {
            return await _bookRepository.GetBook(id);
        }

        public async Task<Book> UpdateBook(Book book)
        {
            _bookRepository.UpdatBook(book);
            await _bookRepository.SaveAsync();
            return book;
        }

        public async Task<Book> DeleteBookAsync(int id)
        {
            var book = await _bookRepository.GetBook(id);
            if (book == null)
            {
                throw new Exception("Book not found");
            }
            _bookRepository.DeletBook(book);
            await _bookRepository.SaveAsync();
            return book;
        }

    }
}
