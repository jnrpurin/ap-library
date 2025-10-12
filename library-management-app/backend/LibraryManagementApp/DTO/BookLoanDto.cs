namespace LibraryManagementApp.DTO
{
    public class BookLoanDto
    {
        public required Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public required Guid BookId { get; set; }
        public string BookTitle { get; set; } = null!;
        public required DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }
    }
}