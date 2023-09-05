using LibraryManagementApp.Data.Base;
using LibraryManagementApp.Data.ViewModels;
using LibraryManagementApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementApp.Data.Services
{
    public interface IBookIssueService : IEntityBaseRepository<BookIssue>
    {
        Task<BookIssueDropdownsVM> GetBookIssueDropdownsValues();
        Task AddNewBookIssueAsync(BookIssueVM bookIssueVM);
        Task UpdateBookIssueAsync(EditBookIssueVM editBookIssueVM);
        Task ReturnBookAsync(BookIssue bookIssue);
    }
}
