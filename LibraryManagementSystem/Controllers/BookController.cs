using LMS.Domain.DTOs.Book;
using LMS.Domain.IService;
using LMS.Domain.Models;
using LMS.Domain.Ultilities;
using Microsoft.AspNetCore.Authorization;
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
                var (booksList, count) = await _bookService.GetBooksAndCountAsync(filter);

                // Prepare the response
                var data = new
                {
                    NumberOfPage = Math.Ceiling(count * 1f / filter.PageSize),
                    Books = booksList
                };

                // Return the response as JSON
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
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult<Book>> AddBook(BookDTO bookDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _bookService.AddBook(bookDTO);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.Error);
                }
                return CreatedAtAction(nameof(GetBook), new { id = result.Value.Id }, result.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Librarian")]
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
        [Authorize(Roles = "Librarian")]
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

    }
}
