using LibraryManagementApp.Models;

namespace LibraryManagementApp.Data.Services
{
    public interface IOrdersService
    {
        Task StoreOrderAsync(List<BusketItems> items, string userId, string userEmailAddress);
        Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole);
    }
}
