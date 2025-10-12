using LibraryManagementApp.Interfaces;
using LibraryManagementApp.Models;
using LibraryManagementApp.Services;
using Moq;

namespace UnitTests.ServiceTests
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _bookService = new BookService(_bookRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllBooksAsync_ShouldReturnAllBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new() { Id = Guid.NewGuid(), Title = "Book 1" },
                new() { Id = Guid.NewGuid(), Title = "Book 2" }
            };

            _bookRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(books);

            // Act
            var result = await _bookService.GetAllBooksAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<Book>)result).Count);
            _bookRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetBookByIdAsync_ShouldReturnBook_WhenBookExists()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var expectedBook = new Book { Id = bookId, Title = "Test Book" };

            _bookRepositoryMock.Setup(r => r.GetByIdAsync(bookId))
                .ReturnsAsync(expectedBook);

            // Act
            var result = await _bookService.GetBookByIdAsync(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Book", result.Title);
            _bookRepositoryMock.Verify(r => r.GetByIdAsync(bookId), Times.Once);
        }

        [Fact]
        public async Task GetBookByIdAsync_ShouldReturnNull_WhenBookDoesNotExist()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            _bookRepositoryMock.Setup(r => r.GetByIdAsync(bookId))
                .ReturnsAsync((Book?)null);

            // Act
            var result = await _bookService.GetBookByIdAsync(bookId);

            // Assert
            Assert.Null(result);
            _bookRepositoryMock.Verify(r => r.GetByIdAsync(bookId), Times.Once);
        }

        [Fact]
        public async Task AddBookAsync_ShouldCallRepositoryAddBookAsync()
        {
            // Arrange
            var newBook = new Book { Id = Guid.NewGuid(), Title = "New Book" };

            // Act
            await _bookService.AddBookAsync(newBook);

            // Assert
            _bookRepositoryMock.Verify(r => r.AddAsync(newBook), Times.Once);
        }

        [Fact]
        public async Task UpdateBookAsync_ShouldCallRepositoryUpdateBookAsync()
        {
            // Arrange
            var existingBook = new Book { Id = Guid.NewGuid(), Title = "Updated Book" };

            // Act
            await _bookService.UpdateBookAsync(existingBook);

            // Assert
            _bookRepositoryMock.Verify(r => r.Update(existingBook), Times.Once);
        }

        [Fact]
        public async Task DeleteBookAsync_ShouldCallRepositoryDeleteBookAsync()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var existingBook = new Book { Id = bookId, Title = "Book to Delete" };
            _bookRepositoryMock.Setup(r => r.GetByIdAsync(bookId))
                .ReturnsAsync(existingBook);

            // Act
            await _bookService.DeleteBookAsync(bookId);

            // Assert
            _bookRepositoryMock.Verify(r => r.Remove(existingBook), Times.Once);
        }
    }
}
