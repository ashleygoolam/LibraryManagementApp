using LibraryManagementApp.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementApp.Data.ViewModels
{
    public class NewBookVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Title is required!")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "The Book category should be specified!")]
        public BookCategory bookCategory { get; set; }
        
        [Required(ErrorMessage = "The Description is required!")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "The Date of creation should be provided!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "The Price is required!")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The Location of the Book must be provided!")]
        public string? Location { get; set; }

        public IFormFile? Image { get; set; }
        public string? ImagePreview { get; set; }

        //Relationships
        [Display(Name = "Select a Publisher")]
        [Required(ErrorMessage = "The Book Publisher is required!")]
        public int PublisherId { get; set; }

        [Display(Name = "Select the Author(s)")]
        [Required(ErrorMessage = "The Book Authors are required!")]
        public List<int>? AuthorIds { get; set; }

    }
}
