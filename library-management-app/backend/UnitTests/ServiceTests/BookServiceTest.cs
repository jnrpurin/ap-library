using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagementApp.Interfaces;
using LibraryManagementApp.Models;
using LibraryManagementApp.Services;
using Moq;
using Xunit;

namespace LibraryManagementApp.Tests.Services
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

            _bookRepositoryMock.Setup(r => r.GetAllBooksAsync())
                .ReturnsAsync(books);

            // Act
            var result = await _bookService.GetAllBooksAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<Book>)result).Count);
            _bookRepositoryMock.Verify(r => r.GetAllBooksAsync(), Times.Once);
        }

        [Fact]
        public async Task GetBookByIdAsync_ShouldReturnBook_WhenBookExists()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var expectedBook = new Book { Id = bookId, Title = "Test Book" };

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(bookId))
                .ReturnsAsync(expectedBook);

            // Act
            var result = await _bookService.GetBookByIdAsync(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Book", result.Title);
            _bookRepositoryMock.Verify(r => r.GetBookByIdAsync(bookId), Times.Once);
        }

        [Fact]
        public async Task GetBookByIdAsync_ShouldReturnNull_WhenBookDoesNotExist()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(bookId))
                .ReturnsAsync((Book)null);

            // Act
            var result = await _bookService.GetBookByIdAsync(bookId);

            // Assert
            Assert.Null(result);
            _bookRepositoryMock.Verify(r => r.GetBookByIdAsync(bookId), Times.Once);
        }

        [Fact]
        public async Task AddBookAsync_ShouldCallRepositoryAddBookAsync()
        {
            // Arrange
            var newBook = new Book { Id = Guid.NewGuid(), Title = "New Book" };

            // Act
            await _bookService.AddBookAsync(newBook);

            // Assert
            _bookRepositoryMock.Verify(r => r.AddBookAsync(newBook), Times.Once);
        }

        [Fact]
        public async Task UpdateBookAsync_ShouldCallRepositoryUpdateBookAsync()
        {
            // Arrange
            var existingBook = new Book { Id = Guid.NewGuid(), Title = "Updated Book" };

            // Act
            await _bookService.UpdateBookAsync(existingBook);

            // Assert
            _bookRepositoryMock.Verify(r => r.UpdateBookAsync(existingBook), Times.Once);
        }

        [Fact]
        public async Task DeleteBookAsync_ShouldCallRepositoryDeleteBookAsync()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            // Act
            await _bookService.DeleteBookAsync(bookId);

            // Assert
            _bookRepositoryMock.Verify(r => r.DeleteBookAsync(bookId), Times.Once);
        }
    }
}
