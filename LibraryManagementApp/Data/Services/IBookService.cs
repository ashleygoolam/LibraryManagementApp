using LibraryManagementApp.Data.Base;
using LibraryManagementApp.Data.ViewModels;
using LibraryManagementApp.Models;

namespace LibraryManagementApp.Data.Services
{
    public interface IBookService : IEntityBaseRepository<Book>
    {
        Task<Book> GetBookByIdAsync(int id);
        Task<NewBookDropdownsVM> GetNewBookDropdownsValues();
        Task AddNewBookAsync(NewBookVM data);
        Task UpdateBookAsync(NewBookVM data);
    }
}
