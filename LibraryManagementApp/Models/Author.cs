using LibraryManagementApp.Data.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApp.Models
{
    public class Author : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The FullName of the Author must be provided")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The Fullname must be between 3 and 60 characters")]
        public string? FullName { get; set; }

        [Display(Name = "ProfilePicture")]
        public string? ProfilePicture { get; set; }

        [Required(ErrorMessage = "The Biography must be provided")]
        public string? Biography { get; set; }

        //Relationships
        public List<Author_Book>? Author_Book { get; set; }
    }
}
