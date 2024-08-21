using LMS.Domain.DTOs.Book;
using LMS.Domain.IService;
using LMS.Domain.Models;
using LMS.Domain.Ultilities;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks([FromQuery] BookFilter filter)
        {
            try
            {
                var books = await _bookService.GetAllBooks(filter);
                var total = await _bookService.GetTotalNumberOfBook();
                var data = new
                {
                    total = Math.Ceiling(total * 1f / filter.PageSize),
                    books
                };
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            try
            {
                var book = await _bookService.GetBook(id);
                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Book>> AddBook(BookDTO bookDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var newBook = await _bookService.AddBook(bookDTO);
                return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Book>> UpdateBook(int id, BookDTO bookDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedBook = await _bookService.UpdateBook(id, bookDTO);
                return Ok(updatedBook);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            try
            {
                var deletedBook = await _bookService.DeleteBookAsync(id);
                return Ok(deletedBook);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("total")]
        public async Task<ActionResult<int>> GetTotalNumberOfBook()
        {
            try
            {
                var total = await _bookService.GetTotalNumberOfBook();
                return Ok(total);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
