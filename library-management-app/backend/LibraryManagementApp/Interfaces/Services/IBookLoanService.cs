using LibraryManagementApp.Shared;
using LibraryManagementApp.Models;

namespace LibraryManagementApp.Interfaces
{
    public interface IBookLoanService
    {
        Task<IEnumerable<BookLoan>> GetAllLoansAsync();
        Task<BookLoan?> GetLoanByIdAsync(Guid id);
        Task<Result<BookLoan>> AddLoanAsync(Guid userId, Guid bookId);
        Task ReturnLoanBookAsync(Guid id);
        Task<IEnumerable<BookLoan>> GetLoansByClient();
    }
}