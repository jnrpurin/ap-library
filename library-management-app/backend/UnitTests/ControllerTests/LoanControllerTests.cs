using LibraryManagementApp.Controllers;
using LibraryManagementApp.DTO;
using LibraryManagementApp.Interfaces;
using LibraryManagementApp.Models;
using LibraryManagementApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace LibraryManagementApp.Tests.Controllers
{
    public class LoanControllerTests
    {
        private readonly Mock<IBookLoanService> _loanServiceMock;
        private readonly Mock<ILogger<LoanController>> _loggerMock;
        private readonly LoanController _controller;

        public LoanControllerTests()
        {
            _loanServiceMock = new Mock<IBookLoanService>();
            _loggerMock = new Mock<ILogger<LoanController>>();
            _controller = new LoanController(_loanServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllLoans_ShouldReturnOk_WithListOfLoans()
        {
            // Arrange
            var loans = new List<BookLoan>
            {
                new() { Id = Guid.NewGuid(), BookId = Guid.NewGuid(), UserId = Guid.NewGuid() }
            };

            _loanServiceMock.Setup(s => s.GetAllLoansAsync())
                            .ReturnsAsync(loans);

            // Act
            var result = await _controller.GetAllLoans();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<BookLoan>>(okResult.Value);
            Assert.Single(value);
        }

        [Fact]
        public async Task GetLoanById_ShouldReturnNotFound_WhenLoanDoesNotExist()
        {
            // Arrange
            var loanId = Guid.NewGuid();
            _loanServiceMock.Setup(s => s.GetLoanByIdAsync(loanId))
                            .ReturnsAsync((BookLoan?)null);

            // Act
            var result = await _controller.GetLoanById(loanId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetLoanById_ShouldReturnOk_WhenLoanExists()
        {
            // Arrange
            var loan = new BookLoan { Id = Guid.NewGuid(), BookId = Guid.NewGuid(), UserId = Guid.NewGuid() };
            _loanServiceMock.Setup(s => s.GetLoanByIdAsync(loan.Id))
                            .ReturnsAsync(loan);

            // Act
            var result = await _controller.GetLoanById(loan.Id);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var returnedLoan = Assert.IsType<BookLoan>(ok.Value);
            Assert.Equal(loan.Id, returnedLoan.Id);
        }

        [Fact]
        public async Task CreateBookLoan_ShouldReturnBadRequest_WhenServiceFails()
        {
            // Arrange
            var dto = new CreateBookLoanDto { UserId = Guid.NewGuid(), BookId = Guid.NewGuid() };
            var serviceResult = Result<BookLoan>.Failure("Invalid data");

            _loanServiceMock.Setup(s => s.AddLoanAsync(dto.UserId, dto.BookId)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.CreateBookLoan(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid data", badRequest.Value);
        }

        [Fact]
        public async Task CreateBookLoan_ShouldReturnCreated_WhenSuccessful()
        {
            // Arrange
            var dto = new CreateBookLoanDto { UserId = Guid.NewGuid(), BookId = Guid.NewGuid() };
            var loan = new BookLoan
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                BookId = dto.BookId,
                LoanDate = DateTime.UtcNow
            };

            var serviceResult = Result<BookLoan>.Success(loan);

            _loanServiceMock.Setup(s => s.AddLoanAsync(dto.UserId, dto.BookId)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.CreateBookLoan(dto);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            var loanDto = Assert.IsType<BookLoanDto>(created.Value);
            Assert.Equal(loan.Id, loanDto.Id);
        }

        [Fact]
        public async Task ReturnBookLoan_ShouldReturnBadRequest_WhenIdMismatch()
        {
            // Arrange
            var id = Guid.NewGuid();
            var loan = new BookLoan { Id = Guid.NewGuid() }; // diferente

            // Act
            var result = await _controller.ReturnBookLoan(id, loan);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task ReturnBookLoan_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var id = Guid.NewGuid();
            var loan = new BookLoan { Id = id };

            _loanServiceMock.Setup(s => s.ReturnLoanBookAsync(id))
                            .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ReturnBookLoan(id, loan);

            // Assert
            Assert.IsType<OkResult>(result);
            _loanServiceMock.Verify(s => s.ReturnLoanBookAsync(id), Times.Once);
        }
    }

    /// <summary>
    /// Classe auxiliar usada para simular o retorno de serviços.
    /// </summary>
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }
    }
}
