using LibraryManagementApp.Models;
using Microsoft.EntityFrameworkCore;
using System;


namespace LibraryManagementApp.Data.Cart
{
    public class Busket
    {
        public ApplicationDbContext _context { get; set; }

        public string BusketId { get; set; }
        public List<BusketItems> BusketItems { get; set; }

        public Busket(ApplicationDbContext context)
        {
            _context = context;
        }

        public static Busket GetBusket(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<ApplicationDbContext>();

            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();
            session?.SetString("CartId", cartId);

            return new Busket(context) { BusketId = cartId };
        }

        public void AddItemToBusket(Book book)
        {
            var busketItems = _context.BusketItems.FirstOrDefault(n => n.Book.Id == book.Id && n.BusketId == BusketId);

            if (busketItems == null)
            {
                busketItems = new BusketItems()
                {
                    BusketId = BusketId,
                    Book = book,
                    Amount = 1
                };

                _context.BusketItems.Add(busketItems);
            }
            else
            {
                busketItems.Amount++;
            }
            _context.SaveChanges();
        }

        public void RemoveItemFromBusket(Book book)
        {
            var busketItems = _context.BusketItems.FirstOrDefault(n => n.Book.Id == book.Id && n.BusketId == BusketId);

            if (busketItems != null)
            {
                if (busketItems.Amount > 1)
                {
                    busketItems.Amount--;
                }
                else
                {
                    _context.BusketItems.Remove(busketItems);
                }
            }
            _context.SaveChanges();
        }

        public List<BusketItems> GetBusketItems()
        {
            return BusketItems ?? (BusketItems = _context.BusketItems.Where(n => n.BusketId == BusketId).Include(n => n.Book).ToList());
        }

        public double GetBusketTotal() => (double)_context.BusketItems.Where(n => n.BusketId == BusketId).Select(n => n.Book.Price * n.Amount).Sum();

        public async Task ClearBusketAsync()
        {
            var items = await _context.BusketItems.Where(n => n.BusketId == BusketId).ToListAsync();
            _context.BusketItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}
