using LibraryManagementApp.Data;
using LibraryManagementApp.Interfaces;
using LibraryManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApp.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
            => await _context.Books.Where(b => b.IsAvailable).ToListAsync();
    }
}