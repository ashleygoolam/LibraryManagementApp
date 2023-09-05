using LibraryManagementApp.Models;

namespace LibraryManagementApp.Data.ViewModels
{
    public class BookInstanceDropDown
    {
        public BookInstanceDropDown() 
        {
            Books = new List<Book>();
        }

        public List<Book> Books { get; set; }
    }
}
