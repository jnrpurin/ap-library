using LibraryManagementApp.DTO;
using LibraryManagementApp.Interfaces;
using LibraryManagementApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementApp.Controllers
{
    /// <summary>
    /// API controller for managing books loan in the library.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly IBookLoanService _loanService;
        private readonly ILogger<LoanController> _logger;

        public LoanController(IBookLoanService loanService, ILogger<LoanController> logger)
        {
            _loanService = loanService;
            _logger = logger;
        }

        /// <summary>
        /// Method to get all loans.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "User_Admin,User_Standard,User_ReadOnly")]
        public async Task<ActionResult<IEnumerable<BookLoan>>> GetAllLoans()
        {
            _logger.LogInformation("Fetching all book loans...");
            var loans = await _loanService.GetAllLoansAsync();
            return Ok(loans);
        }

        /// <summary>
        /// Method to get a loan by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "User_Admin,User_Standard,User_ReadOnly")]
        public async Task<ActionResult<BookLoan>> GetLoanById(Guid id)
        {
            _logger.LogInformation("Fetching loan with ID: {LoanId}", id);
            var loan = await _loanService.GetLoanByIdAsync(id);
            if (loan == null)
            {
                _logger.LogWarning("Loan with ID {LoanId} not found", id);
                return NotFound();
            }
            return Ok(loan);
        }

        /// <summary>
        /// Method to get loans by the currently authenticated customer.
        /// </summary>
        /// <returns></returns>
        [HttpGet("by-customer")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "User_Admin,User_Standard,User_ReadOnly,Member_Client")]
        public async Task<ActionResult<BookLoan>> GetLoansByCustomer()
        {
            _logger.LogInformation("Fetching loans by authenticated customer...");
            var loan = await _loanService.GetLoansByClient();
            if (!loan.Any())
            {
                _logger.LogWarning("No loans found for the current customer");
                return NotFound();
            }
            return Ok(loan);
        }

        /// <summary>
        /// New loan creation method.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "User_Admin,User_Standard")]
        public async Task<ActionResult<Book>> CreateBookLoan([FromBody] CreateBookLoanDto dto)
        {
            try
            {
                _logger.LogInformation("Creating new loan: User={UserId}, Book={BookId}", dto.UserId, dto.BookId);
                var result = await _loanService.AddLoanAsync(dto.UserId, dto.BookId);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to create loan: {Error}", result.ErrorMessage);
                    return BadRequest(result.ErrorMessage);
                }

                var loan = result.Data!;
                _logger.LogInformation("Loan {LoanId} created successfully", loan.Id);

                var loanDto = new BookLoanDto
                {
                    Id = loan.Id,
                    UserId = loan.UserId,
                    UserName = loan.User?.Username ?? "",
                    BookId = loan.BookId,
                    BookTitle = loan.Book?.Title ?? "",
                    LoanDate = loan.LoanDate,
                    ReturnDate = loan.ReturnDate,
                    IsReturned = loan.IsReturned
                };

                return CreatedAtAction(nameof(GetLoanById), new { id = loan.Id }, loanDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new loan for User={UserId}, Book={BookId}", dto.UserId, dto.BookId);
                return StatusCode(500, "An internal error occurred while creating the loan.");
            }
        }

        /// <summary>
        /// Method to update an existing loan.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loan"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "User_Admin,User_Standard")]
        public async Task<IActionResult> ReturnBookLoan(Guid id, BookLoan loan)
        {
            if (id != loan.Id)
            {
                _logger.LogWarning("Loan ID mismatch: route={IdRoute}, body={IdBody}", id, loan.Id);
                return BadRequest();
            }

            try
            {
                await _loanService.ReturnLoanBookAsync(loan.Id);
                _logger.LogInformation("Loan {LoanId} marked as returned", id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error returning loan {LoanId}", id);
                return StatusCode(500, "An internal error occurred while returning the loan.");
            }
        }
    }
}