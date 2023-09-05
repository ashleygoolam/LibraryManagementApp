using LibraryManagementApp.Models;

namespace LibraryManagementApp.Data.ViewModels
{
    public class BookIssueDropdownsVM
    {
        public BookIssueDropdownsVM()
        {
            BookInstances = new List<BookInstance>();
        }

        public List<BookInstance> BookInstances { get; set; }
    }
}
