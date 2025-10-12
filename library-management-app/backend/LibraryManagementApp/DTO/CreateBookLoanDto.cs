namespace LibraryManagementApp.DTO
{
    public class CreateBookLoanDto
    {
        public required Guid UserId { get; set; }
        public required Guid BookId { get; set; }
    }
}