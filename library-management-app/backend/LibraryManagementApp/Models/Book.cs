namespace LibraryManagementApp.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}