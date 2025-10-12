using LibraryManagementApp.Shared;
using LibraryManagementApp.Interfaces;
using LibraryManagementApp.Models;

namespace LibraryManagementApp.Services
{
    public class BookLoanService(IBookLoanRepository loanRepository, IBookRepository bookRepository, IAuthService authService) : IBookLoanService
    {
        private readonly IBookLoanRepository _loanRepository = loanRepository;
        private readonly IBookRepository _bookRepository = bookRepository;
        private readonly IAuthService _authService = authService;

        public async Task<IEnumerable<BookLoan>> GetAllLoansAsync()
        {
            return await _loanRepository.GetAllAsync();
        }

        public async Task<BookLoan?> GetLoanByIdAsync(Guid id)
        {
            return await _loanRepository.GetByIdAsync(id);
        }

        public async Task<Result<BookLoan>> AddLoanAsync(Guid userId, Guid bookId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
                return Result<BookLoan>.Failure("Book not found.");

            if (!book.IsAvailable)
                return Result<BookLoan>.Failure("Book is not available for loan.");

            var bookLoan = new BookLoan
            {
                UserId = userId,
                BookId = bookId,
                LoanDate = DateTime.UtcNow,
                ReturnDate = null
            };
            await _loanRepository.AddAsync(bookLoan);

            book.IsAvailable = false;
            _bookRepository.Update(book);

            await _loanRepository.SaveChangesAsync();

            return Result<BookLoan>.Success(bookLoan);
        }

        public async Task ReturnLoanBookAsync(Guid id)
        {
            var loan = await _loanRepository.GetByIdAsync(id);
            if (loan != null)
            {
                loan.ReturnDate = DateTime.UtcNow;
                _loanRepository.Update(loan);
                await _loanRepository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<BookLoan>> GetLoansByClient()
        {
            Guid customerId = _authService.GetAuthenticatedUserId();
            return await _loanRepository.GetLoansByClient(customerId);
        }
    }
}