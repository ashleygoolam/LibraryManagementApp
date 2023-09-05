using LibraryManagementApp.Data.Base;
using LibraryManagementApp.Data.ViewModels;
using LibraryManagementApp.Models;

namespace LibraryManagementApp.Data.Services
{
    public interface IAuthorsService : IEntityBaseRepository<Author>
    {
        Task UpdateAuthorAsync(NewAuthorVM data);
        Task AddNewAuthorAsync(NewAuthorVM data);
    }
}
