using LibraryManagementApp.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LibraryManagementApp.Data.Base;

namespace LibraryManagementApp.Models
{
    public class BookIssue : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public string? BorrowersName { get; set; }
        public string? BorrowingLibrariansName { get; set; }
        public string? ReturningLibrariansName { get; set; }
        public DateTime TimeBorrowed { get; set; }
        public DateTime TimeReturned { get; set; }
        public DateTime DueDate { get; set; }
        public BookStatus bookStatus { get; set; }
        public BookReturnStatus bookReturnStatus { get; set; }

        //Relationships
        public int BookInstanceId { get; set; }
        [ForeignKey("BookInstanceId")]
        public BookInstance? BookInstance { get; set; }

        //public int CardId { get; set; }
        //public LibraryCard? Card { get; set; }
    }
}
