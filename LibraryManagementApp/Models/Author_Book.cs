namespace LibraryManagementApp.Models
{
    public class Author_Book
    {
        public int AuthorId { get; set; }
        public Author? Author { get; set; }

        public int BookId { get; set; }
        public Book? Book { get; set; }
    }
}
