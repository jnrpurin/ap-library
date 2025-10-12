using LibraryManagementApp.Data;
using LibraryManagementApp.Interfaces;
using LibraryManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApp.Repositories
{
    public class BookLoanRepository : BaseRepository<BookLoan>, IBookLoanRepository
    {
        public BookLoanRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<BookLoan>> GetActiveLoansByUserAsync(Guid userId)
        {
            var loans = await GetLoansByClient(userId);
            return loans.Where(bl => bl.ReturnDate == null);
        }

        public async Task<IEnumerable<BookLoan>> GetLoansByClient(Guid clientUserId)
        {
            return await _context.BookLoans
                .Where(bl => bl.UserId == clientUserId)
                .Include(bl => bl.User)
                .Include(bl => bl.Book)
                .ToListAsync();
        }
    }
}