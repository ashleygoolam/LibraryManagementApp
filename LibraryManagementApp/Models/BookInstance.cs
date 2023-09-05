using LibraryManagementApp.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LibraryManagementApp.Data.Base;

namespace LibraryManagementApp.Models
{
    public class BookInstance : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Isbn Number should be Provided!")]
        public string? IsbnNumber { get; set; }

        [Required(ErrorMessage = "The Status should be stated!")]
        public BookStatus bookStatus { get; set; }

        [Required(ErrorMessage = "The Availability should be stated!")]
        public BookAvailability bookAvailability { get; set; }

        //Relationships
        [Required(ErrorMessage = "The Book should be specified!")]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book? Book { get; set; }

        public List<BookIssue>? BookIssue { get; set; }
    }
}
