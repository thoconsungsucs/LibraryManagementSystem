using LMS.Domain.DTOs.Book;
using LMS.Domain.Exceptions;
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
                var bookResult = await _bookService.GetBook(id);
                return bookResult.IsSuccess ? bookResult.Value : NotFound(ApiResult.ToProblemDetails(bookResult));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> AddBook(BookDTO bookDTO)
        {
            try
            {
                var createdBookResult = await _bookService.AddBook(bookDTO);
                return createdBookResult.IsSuccess ?
                    CreatedAtAction(nameof(GetBook), new { id = createdBookResult.Value.Id }, createdBookResult.Value) :
                    BadRequest(ApiResult.ToProblemDetails(createdBookResult));

            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error with the exception message
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpPut("{id}")]
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult<Book>> UpdateBook(int id, BookDTO bookDTO)
        {
            try
            {
                var updatedBookResult = await _bookService.UpdateBook(id, bookDTO);
                return updatedBookResult.IsSuccess ? Ok(updatedBookResult.Value) : NotFound(ApiResult.ToProblemDetails(updatedBookResult));
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
                var deletedBookResult = await _bookService.DeleteBookAsync(id);
                return deletedBookResult.IsSuccess ? NoContent() : NotFound(ApiResult.ToProblemDetails(deletedBookResult));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
