using LibraryManagementApp.Data.Base;
using LibraryManagementApp.Data.ViewModels;
using LibraryManagementApp.Models;

namespace LibraryManagementApp.Data.Services
{
    public interface IPublishersService : IEntityBaseRepository<Publisher>
    {
        Task UpdatePublisherAsync(NewPublisherVM data);
        Task AddNewPublisherAsync(NewPublisherVM data);
    }
}
