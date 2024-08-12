using LMS.DataAccess.Data;
using LMS.Domain.IRepository;
using LMS.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.DataAccess.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;
        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<Book> GetAllBooks()
        {
            return _context.Books;
        }

        public async Task<Book> GetBook(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        }
        public void DeletBook(Book book)
        {
            _context.Books.Remove(book);
        }

        public void UpdatBook(Book book)
        {
            _context.Books.Update(book);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
