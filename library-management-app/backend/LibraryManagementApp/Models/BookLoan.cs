namespace LibraryManagementApp.Models
{
    public class BookLoan
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public Guid BookId { get; set; }
        public Book? Book { get; set; }

        public DateTime LoanDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnDate { get; set; } = null;
        public bool IsReturned => ReturnDate.HasValue;
    }
}
