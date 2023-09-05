using LibraryManagementApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApp.Data.ViewModels
{
    public class NewPublisherVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Publisher's name should be provided!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The name must be between 3 and 60 characters")]
        public string? PublisherName { get; set; }

        public string? PublisherLogoPreview { get; set; }
        public IFormFile? PublisherLogo { get; set; }

        [Required(ErrorMessage = "The Publisher's description should be provided!")]
        public string? PublisherDescription { get; set; }

        [Required(ErrorMessage = "The Rating should be specified!")]
        public Rating Rating { get; set; }
    }
}
