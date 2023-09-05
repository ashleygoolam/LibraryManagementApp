using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LibraryManagementApp.Enums;
using LibraryManagementApp.Data.Base;

namespace LibraryManagementApp.Models
{
    public class Book : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public BookCategory bookCategory { get; set; }
        public string? Description { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }
        public int AvailableAmount { get; set; }
        public int TotalAmount { get; set; }
        //[Range(1, 100)]
        //[DataType(DataType.Currency)]
        //[Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public string? Location { get; set; }
        public string? Image { get; set; }

        //Relationships
        public List<BookInstance>? BookInstance { get; set; }
        public List<Author_Book>? Author_Book { get; set; }
        public int PublisherId { get; set; }
        [ForeignKey("PublisherId")]
        public Publisher? Publisher { get; set; }
    }
}
