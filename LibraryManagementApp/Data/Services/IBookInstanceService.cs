using LibraryManagementApp.Data.Base;
using LibraryManagementApp.Data.ViewModels;
using LibraryManagementApp.Models;

namespace LibraryManagementApp.Data.Services
{
    public interface IBookInstanceService : IEntityBaseRepository<BookInstance>
    {
        Task<BookInstanceDropDown> GetBookInstanceDropDownsValues();
        Task AddNewBookInstanceAsync(BookInstance bookInstance);
        Task UpdateBookInstanceAsync(BookInstance bookInstance);
    }
}
