using LibraryManagementApp.Models;

namespace LibraryManagementApp.Interfaces
{
    public interface IBookLoanRepository : IBaseRepository<BookLoan>
    {
        Task<IEnumerable<BookLoan>> GetActiveLoansByUserAsync(Guid userId);
        Task<IEnumerable<BookLoan>> GetLoansByClient(Guid clientUserId);
    }
}