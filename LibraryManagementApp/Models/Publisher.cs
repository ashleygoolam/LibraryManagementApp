using LibraryManagementApp.Data.Base;
using LibraryManagementApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApp.Models
{
    public class Publisher : IEntityBase
    {
        public int Id { get; set; }

        [Required]
        public string? PublisherName { get; set; }

        public string? PublisherLogo { get; set; }

        [Required]
        public string? PublisherDescription { get; set; }

        [Required]
        public Rating Rating { get; set; }

        //Relationships
        public List<Book>? Books { get; set; }
    }
}
