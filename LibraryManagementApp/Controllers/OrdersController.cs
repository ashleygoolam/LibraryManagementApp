using LibraryManagementApp.Data.Cart;
using LibraryManagementApp.Data.Services;
using LibraryManagementApp.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagementApp.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IBookService _bookService;
        private readonly Busket _busket;
        private readonly IOrdersService _ordersService;

        public OrdersController(IBookService bookService, Busket busket, IOrdersService ordersService)
        {
            _bookService = bookService;
            _busket = busket;
            _ordersService = ordersService;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            var orders = await _ordersService.GetOrdersByUserIdAndRoleAsync(userId, userRole);
            return View(orders);
        }

        public IActionResult Busket()
        {
            var items = _busket.GetBusketItems();
            _busket.BusketItems = items;

            var response = new BusketVM()
            {
                Busket = _busket,
                BusketTotal = _busket.GetBusketTotal()
            };

            return View(response);
        }

        public async Task<IActionResult> AddItemToBusket(int id)
        {
            var item = await _bookService.GetBookByIdAsync(id);

            if (item != null)
            {
                _busket.AddItemToBusket(item);
            }
            return RedirectToAction(nameof(Busket));
        }

        public async Task<IActionResult> RemoveItemFromBusket(int id)
        {
            var item = await _bookService.GetBookByIdAsync(id);

            if (item != null)
            {
                _busket.RemoveItemFromBusket(item);
            }
            return RedirectToAction(nameof(Busket));
        }

        public async Task<IActionResult> CompleteOrder()
        {
            var items = _busket.GetBusketItems();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userEmailAddress = User.FindFirstValue(ClaimTypes.Email);

            await _ordersService.StoreOrderAsync(items, userId, userEmailAddress);
            await _busket.ClearBusketAsync();

            return View("OrderCompleted");
        }
    }
}
