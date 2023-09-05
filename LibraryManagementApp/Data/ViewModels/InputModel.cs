using LibraryManagementApp.Models;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApp.Data.ViewModels
{
    public class InputModel
    {
        [Phone]
        [Display(Name = "Phone number")]
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public string? ProfilePicture { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
