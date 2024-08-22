using FluentValidation;
using LMS.Domain.DTOs.Book;
using LMS.Domain.IRepository;
using LMS.Domain.IService;
using LMS.Domain.Mappers;
using LMS.Domain.Models;
using LMS.Domain.Ultilities;
using Microsoft.EntityFrameworkCore;
namespace LMS.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IValidator<BookDTO> _bookValidator;
        public BookService(IBookRepository bookRepository, IValidator<BookDTO> bookValidator)
        {
            _bookRepository = bookRepository;
            _bookValidator = bookValidator;
        }
        public async Task<(List<Book>, int)> GetBooksAndCountAsync(BookFilter filter)
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

            if (filter.Pages != null && filter.Pages != 0)
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

            var count = await books.CountAsync();
            books = books.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);

            var booksList = await books.ToListAsync();
            return (booksList, count);
        }

        public async Task<Book> GetBook(int id)
        {
            return await _bookRepository.GetBook(id);
        }

        public async Task<Book> AddBook(BookDTO bookDTO)
        {
            var validationResult = await _bookValidator.ValidateAsync(bookDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var book = bookDTO.ToBook();
            _bookRepository.AddBook(book);
            await _bookRepository.SaveAsync();
            return book;
        }

        public async Task<Book> UpdateBook(int id, BookDTO bookDTO)
        {
            var book = await _bookRepository.GetBook(id);
            if (book == null)
            {
                throw new Exception("Book not found");
            }

            var validationResult = await _bookValidator.ValidateAsync(bookDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            //Update book
            book.Title = bookDTO.Title;
            book.Author = bookDTO.Author;
            book.Publisher = bookDTO.Publisher;
            book.PublishedDate = bookDTO.PublishedDate;
            book.ISBN = bookDTO.ISBN;
            book.Category = bookDTO.Category;
            book.Pages = bookDTO.Pages;
            book.Description = bookDTO.Description;
            book.Quantity = bookDTO.Quantity;
            book.ImageURL = bookDTO.ImageURL;
            book.Available += bookDTO.Quantity - book.Quantity;

            _bookRepository.UpdateBook(book);
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

        public async Task<int> GetTotalNumberOfBook()
        {
            return await _bookRepository.GetAllBooks().CountAsync();
        }

        public Task<List<Book>> GetAllBooks(BookFilter filter)
        {
            throw new NotImplementedException();
        }


    }
}
