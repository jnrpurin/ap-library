using LibraryManagementApp.Models;

namespace LibraryManagementApp.Interfaces
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        Task<IEnumerable<Book>> GetAvailableBooksAsync();
    }
}