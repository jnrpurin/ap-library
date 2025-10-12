using LibraryManagementApp.Interfaces;
using LibraryManagementApp.Models;

namespace LibraryManagementApp.Services
{
    public class BookService(IBookRepository bookRepository) : IBookService
    {
        private readonly IBookRepository _bookRepository= bookRepository;

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<Book?> GetBookByIdAsync(Guid id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task AddBookAsync(Book book)
        {
            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            _bookRepository.Update(book);
            await _bookRepository.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book != null)
            {
                _bookRepository.Remove(book);
                await _bookRepository.SaveChangesAsync();
            }
        }
    }
}