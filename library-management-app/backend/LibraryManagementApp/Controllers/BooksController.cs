using LibraryManagementApp.Enums;
using LibraryManagementApp.Interfaces;
using LibraryManagementApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementApp.Controllers
{
    /// <summary>
    /// API controller for managing books in the library.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BooksController(IBookService bookService) : ControllerBase
    {
        private readonly IBookService _bookService = bookService;

        /// <summary>
        /// Method to get all books.
        /// </summary>
        /// <returns>A list of all books saved</returns>
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        /// <summary>
        /// Method to get a book by its ID.
        /// </summary>
        /// <param name="id">Book identification</param>
        /// <returns>A book information</returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<Book>> GetBookById(Guid id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        /// <summary>
        /// Method to create a new book.
        /// </summary>
        /// <param name="book">A specific book</param>
        /// <returns>This book details</returns>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Standard,Admin")]
        public async Task<ActionResult<Book>> CreateBook(Book book)
        {
            await _bookService.AddBookAsync(book);
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }

        /// <summary>
        /// Method to update an existing book.
        /// </summary>
        /// <param name="id">Book identification</param>
        /// <param name="book">A book information</param>
        /// <returns>Ok if updated the book, otherwise bad request</returns>
        [HttpPut("{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Standard,Admin")]
        public async Task<IActionResult> UpdateBook(Guid id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            await _bookService.UpdateBookAsync(book);
            return Ok();
        }

        /// <summary>
        /// Method to delete a book by its ID.
        /// </summary>
        /// <param name="id">Book identification</param>
        /// <returns>Ok if deleted the book</returns>
        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            await _bookService.DeleteBookAsync(id);
            return Ok();
        }
    }
}