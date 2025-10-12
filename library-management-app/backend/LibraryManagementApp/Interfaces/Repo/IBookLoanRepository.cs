using LibraryManagementApp.Models;

namespace LibraryManagementApp.Interfaces
{
    public interface IBookLoanRepository : IBaseRepository<BookLoan>
    {
        Task<IEnumerable<BookLoan>> GetActiveLoansByUserAsync(int userId);
    }
}