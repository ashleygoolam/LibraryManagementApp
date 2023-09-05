using LibraryManagementApp.Data.Base;
using LibraryManagementApp.Data.ViewModels;
using LibraryManagementApp.Enums;
using LibraryManagementApp.Models;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace LibraryManagementApp.Data.Services
{
    public class BookInstanceService : EntityBaseRepository<BookInstance>, IBookInstanceService
    {
        private readonly ApplicationDbContext _context;
        public BookInstanceService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddNewBookInstanceAsync(BookInstance bookInstance)
        {
            //Find the Book this particular Instance belongs to
            var parentBook = _context.Book.Find(bookInstance.BookId);

            //Setting book availability based on the book status result
            if (bookInstance.bookStatus == BookStatus.Damaged) {
                bookInstance.bookAvailability = BookAvailability.ReadOnly;
                //Increase the particular book's total amount by 1
                parentBook!.TotalAmount += 1;
            } else {
                bookInstance.bookAvailability = BookAvailability.Available;
                //since the book is not damaged or lost, Increase the particular book's total and available amounts by 1
                parentBook!.TotalAmount += 1;
                parentBook.AvailableAmount += 1;
            }

            var newBookInstance = new BookInstance()
            {
                IsbnNumber = bookInstance.IsbnNumber,
                bookStatus = bookInstance.bookStatus,
                bookAvailability = bookInstance.bookAvailability,
                BookId = bookInstance.BookId
            };

            await _context.BookInstance.AddAsync(newBookInstance);
            await _context.SaveChangesAsync();
        }

        public async Task<BookInstanceDropDown> GetBookInstanceDropDownsValues()
        {
            var response = new BookInstanceDropDown()
            {
                Books = await _context.Book.OrderBy(n => n.Title).ToListAsync()
            };
            return response;
        }

        public async Task UpdateBookInstanceAsync(BookInstance bookInstance)
        {
            //Find the Instance that we want to update
            var dbBookInstance = await _context.BookInstance.FirstOrDefaultAsync(n => n.Id == bookInstance.Id);

            //Find the Parent book this instance belongs to
            var parentBook = _context.Book.Find(bookInstance.BookId);

            //Check previous status of the Instance
            bool unavailable = false;

            if (dbBookInstance!.bookStatus == BookStatus.Damaged || dbBookInstance.bookStatus == BookStatus.Lost)
                unavailable = true;

            //Update the Information
            if (dbBookInstance != null)
            {
                dbBookInstance.IsbnNumber = bookInstance.IsbnNumber;
                dbBookInstance.bookStatus = bookInstance.bookStatus;

                //Setting book availability based on the book status result
                if (bookInstance.bookStatus == BookStatus.Damaged) {
                    dbBookInstance.bookAvailability = BookAvailability.ReadOnly;
                    //decrease the parent book's available amount
                    if (unavailable == false) parentBook!.AvailableAmount -= 1;
                }
                else if (bookInstance.bookStatus == BookStatus.Lost) {
                    dbBookInstance.bookAvailability = BookAvailability.Unavailable;
                    //decrease the parent book's available amount
                    if (unavailable == false) parentBook!.AvailableAmount -= 1;
                } else {
                    dbBookInstance.bookAvailability = BookAvailability.Available;
                    //Increase the particular book's available amount by 1
                    if (unavailable == true) parentBook!.AvailableAmount += 1;
                }

                dbBookInstance.BookId = bookInstance.BookId;
                await _context.SaveChangesAsync();
            }
            await _context.SaveChangesAsync();
        }
    }
}
