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

        public async Task<IEnumerable<BookLoan>> GetActiveLoansByUserAsync(int userId)
        {
            return await _context.BookLoans
                .Where(bl => bl.UserId.Equals(userId) && bl.ReturnDate == null)
                .Include(bl => bl.User)
                .Include(bl => bl.Book)
                .ToListAsync();
        }
    }
}