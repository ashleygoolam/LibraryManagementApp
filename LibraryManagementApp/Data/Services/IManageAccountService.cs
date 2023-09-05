using LibraryManagementApp.Data.Base;
using LibraryManagementApp.Data.ViewModels;
using LibraryManagementApp.Models;

namespace LibraryManagementApp.Data.Services
{
    public interface IManageAccountService
    {
        Task UpdateUserAsync(ManageAccountVM data);
    }
}
