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
    public class LoanController(IBookLoanService loanService) : ControllerBase
    {
        private readonly IBookLoanService _loanService = loanService;

        /// <summary>
        /// Method to get all loans.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "User_Admin,User_Standard,User_ReadOnly")]
        public async Task<ActionResult<IEnumerable<BookLoan>>> GetAllLoans()
        {
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
            var loan = await _loanService.GetLoanByIdAsync(id);
            if (loan == null)
            {
                return NotFound();
            }
            return Ok(loan);
        }

        [HttpGet()]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "User_Admin,User_Standard,User_ReadOnly,Member_Client")]
        public async Task<ActionResult<BookLoan>> GetLoansByCustomer()
        {
            var loan = await _loanService.GetLoansByClient();
            if (!loan.Any())
            {
                return NotFound();
            }
            return Ok(loan);
        }

        /// <summary>
        /// New loan creation method.
        /// </summary>
        /// <param name="createBookLoanDto"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "User_Admin,User_Standard")]
        public async Task<ActionResult<Book>> CreateBookLoan([FromBody] CreateBookLoanDto createBookLoanDto)
        {
            var result = await _loanService.AddLoanAsync(createBookLoanDto.UserId, createBookLoanDto.BookId);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);
            
            var loan = result.Data!;
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
                return BadRequest();
            }

            await _loanService.ReturnLoanBookAsync(loan.Id);
            return Ok();
        }
    }
}