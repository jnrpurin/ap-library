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
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookService bookService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        /// <summary>
        /// Method to get all books.
        /// </summary>
        /// <returns>A list of all books saved</returns>
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            _logger.LogInformation("Fetching all books...");
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
            _logger.LogInformation("Fetching book with ID: {BookId}", id);

            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                _logger.LogWarning("Book with ID {BookId} not found", id);
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
        [Authorize(Roles = "User_Admin,User_Standard")]
        public async Task<ActionResult<Book>> CreateBook(Book book)
        {
            try
            {
                _logger.LogInformation("Creating a new book: {Title}", book.Title);
                await _bookService.AddBookAsync(book);
                _logger.LogInformation("Book created successfully with ID: {BookId}", book.Id);
                return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating book {Title}", book.Title);
                return StatusCode(500, "An internal error occurred while creating the book.");
            }
        }

        /// <summary>
        /// Method to update an existing book.
        /// </summary>
        /// <param name="id">Book identification</param>
        /// <param name="book">A book information</param>
        /// <returns>Ok if updated the book, otherwise bad request</returns>
        [HttpPut("{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "User_Admin,User_Standard")]
        public async Task<IActionResult> UpdateBook(Guid id, Book book)
        {
            if (id != book.Id)
            {
                _logger.LogWarning("Book ID mismatch: route={IdRoute}, body={IdBody}", id, book.Id);
                return BadRequest();
            }

            try
            {
                await _bookService.UpdateBookAsync(book);
                _logger.LogInformation("Book {BookId} updated successfully", id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book {BookId}", id);
                return StatusCode(500, "An internal error occurred while updating the book.");
            }
        }

        /// <summary>
        /// Method to delete a book by its ID.
        /// </summary>
        /// <param name="id">Book identification</param>
        /// <returns>Ok if deleted the book</returns>
        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "User_Admin")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            try
            {
                await _bookService.DeleteBookAsync(id);
                _logger.LogInformation("Book {BookId} deleted successfully", id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book {BookId}", id);
                return StatusCode(500, "An internal error occurred while deleting the book.");
            }
        }
    }
}