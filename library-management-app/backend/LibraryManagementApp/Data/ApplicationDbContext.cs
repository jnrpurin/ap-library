using Microsoft.EntityFrameworkCore;
using LibraryManagementApp.Models;

namespace LibraryManagementApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookLoan> BookLoans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<BookLoan>()
                .HasOne(bl => bl.Book)
                .WithMany()
                .HasForeignKey(bl => bl.BookId);

            modelBuilder.Entity<BookLoan>()
                .HasOne(bl => bl.User)
                .WithMany()
                .HasForeignKey(bl => bl.UserId);
        }
    }
}