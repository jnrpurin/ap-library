using LibraryManagementApp.Controllers;
using LibraryManagementApp.Interfaces;
using LibraryManagementApp.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.ControllerTests
{
    public class BooksControllerTests
    {
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly BooksController _controller;

        public BooksControllerTests()
        {
            _bookServiceMock = new Mock<IBookService>();
            _controller = new BooksController(_bookServiceMock.Object);
        }

        [Fact]
        public async Task GetBooks_ShouldReturnOk_WithBooksList()
        {
            // Arrange
            var books = new List<Book> { new() { Id = Guid.NewGuid(), Title = "Book 1" } };
            _bookServiceMock.Setup(s => s.GetAllBooksAsync()).ReturnsAsync(books);

            // Act
            var result = await _controller.GetBooks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnBooks = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
            Assert.Single(returnBooks);
        }

        [Fact]
        public async Task GetBookById_ShouldReturnOk_WhenBookExists()
        {
            var id = Guid.NewGuid();
            var book = new Book { Id = id, Title = "Test Book" };
            _bookServiceMock.Setup(s => s.GetBookByIdAsync(id)).ReturnsAsync(book);

            var result = await _controller.GetBookById(id);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnBook = Assert.IsType<Book>(okResult.Value);
            Assert.Equal("Test Book", returnBook.Title);
        }

        [Fact]
        public async Task GetBookById_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            var id = Guid.NewGuid();
            _bookServiceMock.Setup(s => s.GetBookByIdAsync(id)).ReturnsAsync((Book?)null);

            var result = await _controller.GetBookById(id);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateBook_ShouldReturnCreatedAtAction()
        {
            var book = new Book { Id = Guid.NewGuid(), Title = "New Book" };

            var result = await _controller.CreateBook(book);

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(_controller.GetBookById), created.ActionName);
        }

        [Fact]
        public async Task UpdateBook_ShouldReturnBadRequest_WhenIdDoesNotMatch()
        {
            var book = new Book { Id = Guid.NewGuid(), Title = "Mismatch" };
            var result = await _controller.UpdateBook(Guid.NewGuid(), book);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteBook_ShouldReturnOk()
        {
            var id = Guid.NewGuid();
            var result = await _controller.DeleteBook(id);
            Assert.IsType<OkResult>(result);
        }
    }
}
