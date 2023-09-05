using LibraryManagementApp.Data.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementApp.Models
{
    public class LibraryCard : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public Guid CardNumber { get; set; }
        public string? Owner { get; set; }
        public decimal Balance { get; set; }
        public bool isActivated { get; set; }

        //Relationships
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        //public List<BookIssue>? BooksIssued { get; set; }
    }
}
