using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApp.Models
{
    public class BusketItems
    {
        [Key]
        public int Id { get; set; }
        public Book? Book { get; set; }
        public int Amount { get; set; }
        public string? BusketId { get; set; }
    }
}
