using LibraryManagementApp.Enums;
using LibraryManagementApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementApp.Data.ViewModels
{
    public class BookIssueVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The borrowers name is required!")]
        public string? BorrowersName { get; set; }

        [Required(ErrorMessage = "The Borrowing Librarians Name is required!")]
        public string? BorrowingLibrariansName { get; set; }

        [Required(ErrorMessage = "The time borrowed should be specified!")]
        public DateTime TimeBorrowed { get; set; }

        [Required(ErrorMessage = "The due date is required!")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "The book status should be specified!")]
        public BookStatus bookStatus { get; set; }

        //Relationships
        [Required(ErrorMessage = "The ISBN number is required!")]
        public int BookInstanceId { get; set; }

    }
}
