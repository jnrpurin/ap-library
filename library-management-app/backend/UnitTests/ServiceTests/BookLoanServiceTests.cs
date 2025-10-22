using LibraryManagementApp.Interfaces;
using LibraryManagementApp.Models;
using LibraryManagementApp.Services;
using Moq;

namespace LibraryManagementApp.Tests.Services
{
    public class BookLoanServiceTests
    {
        private readonly Mock<IBookLoanRepository> _loanRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly BookLoanService _service;

        public BookLoanServiceTests()
        {
            _loanRepositoryMock = new Mock<IBookLoanRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _authServiceMock = new Mock<IAuthService>();

            _service = new BookLoanService(
                _loanRepositoryMock.Object,
                _bookRepositoryMock.Object,
                _authServiceMock.Object
            );
        }

        [Fact]
        public async Task GetAllLoansAsync_ShouldReturnAllLoans()
        {
            // Arrange
            var loans = new List<BookLoan>
            {
                new() { Id = Guid.NewGuid(), BookId = Guid.NewGuid(), UserId = Guid.NewGuid() }
            };

            _loanRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(loans);

            // Act
            var result = await _service.GetAllLoansAsync();

            // Assert
            Assert.Single(result);
            _loanRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetLoanByIdAsync_ShouldReturnLoan_WhenFound()
        {
            // Arrange
            var loan = new BookLoan { Id = Guid.NewGuid() };
            _loanRepositoryMock.Setup(r => r.GetByIdAsync(loan.Id)).ReturnsAsync(loan);

            // Act
            var result = await _service.GetLoanByIdAsync(loan.Id);

            // Assert
            Assert.Equal(loan.Id, result!.Id);
        }

        [Fact]
        public async Task GetLoanByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            _loanRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((BookLoan?)null);

            // Act
            var result = await _service.GetLoanByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddLoanAsync_ShouldFail_WhenBookNotFound()
        {
            // Arrange
            _bookRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Book?)null);

            // Act
            var result = await _service.AddLoanAsync(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Book not found.", result.ErrorMessage);
            _loanRepositoryMock.Verify(r => r.AddAsync(It.IsAny<BookLoan>()), Times.Never);
        }

        [Fact]
        public async Task AddLoanAsync_ShouldFail_WhenBookNotAvailable()
        {
            // Arrange
            var book = new Book { Id = Guid.NewGuid(), IsAvailable = false };
            _bookRepositoryMock.Setup(r => r.GetByIdAsync(book.Id)).ReturnsAsync(book);

            // Act
            var result = await _service.AddLoanAsync(Guid.NewGuid(), book.Id);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Book is not available for loan.", result.ErrorMessage);
            _loanRepositoryMock.Verify(r => r.AddAsync(It.IsAny<BookLoan>()), Times.Never);
        }

        [Fact]
        public async Task AddLoanAsync_ShouldCreateLoan_WhenBookAvailable()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var book = new Book { Id = Guid.NewGuid(), IsAvailable = true };

            _bookRepositoryMock.Setup(r => r.GetByIdAsync(book.Id)).ReturnsAsync(book);

            // Act
            var result = await _service.AddLoanAsync(userId, book.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.False(book.IsAvailable);
            _loanRepositoryMock.Verify(r => r.AddAsync(It.IsAny<BookLoan>()), Times.Once);
            _bookRepositoryMock.Verify(r => r.Update(It.IsAny<Book>()), Times.Once);
            _loanRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ReturnLoanBookAsync_ShouldUpdateLoan_WhenExists()
        {
            // Arrange
            var loan = new BookLoan { Id = Guid.NewGuid(), ReturnDate = null };
            _loanRepositoryMock.Setup(r => r.GetByIdAsync(loan.Id)).ReturnsAsync(loan);

            // Act
            await _service.ReturnLoanBookAsync(loan.Id);

            // Assert
            Assert.NotNull(loan.ReturnDate);
            _loanRepositoryMock.Verify(r => r.Update(loan), Times.Once);
            _loanRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ReturnLoanBookAsync_ShouldDoNothing_WhenLoanNotFound()
        {
            // Arrange
            _loanRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((BookLoan?)null);

            // Act
            await _service.ReturnLoanBookAsync(Guid.NewGuid());

            // Assert
            _loanRepositoryMock.Verify(r => r.Update(It.IsAny<BookLoan>()), Times.Never);
            _loanRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task GetLoansByClient_ShouldReturnLoansForAuthenticatedUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var loans = new List<BookLoan> { new() { Id = Guid.NewGuid(), UserId = userId } };

            _authServiceMock.Setup(a => a.GetAuthenticatedUserId()).Returns(userId);
            _loanRepositoryMock.Setup(r => r.GetLoansByClient(userId)).ReturnsAsync(loans);

            // Act
            var result = await _service.GetLoansByClient();

            // Assert
            Assert.Single(result);
            Assert.Equal(userId, result.First().UserId);
            _authServiceMock.Verify(a => a.GetAuthenticatedUserId(), Times.Once);
            _loanRepositoryMock.Verify(r => r.GetLoansByClient(userId), Times.Once);
        }
    }
}
