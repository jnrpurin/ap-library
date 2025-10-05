// IBookRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagementApp.Models;

namespace LibraryManagementApp.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);
    }
}