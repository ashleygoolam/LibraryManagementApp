using LibraryManagementApp.Data.Base;
using LibraryManagementApp.Data.ViewModels;
using LibraryManagementApp.Enums;
using LibraryManagementApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryManagementApp.Data.Services
{
    public class BookIssueService : EntityBaseRepository<BookIssue>, IBookIssueService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookIssueService(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context) 
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task AddNewBookIssueAsync(BookIssueVM bookIssueVM)
        {
            var newBookIssue = new BookIssue()
            {
                BorrowersName = bookIssueVM.BorrowersName,
                BorrowingLibrariansName = bookIssueVM.BorrowingLibrariansName,
                TimeBorrowed = bookIssueVM.TimeBorrowed,
                DueDate = bookIssueVM.DueDate,
                bookStatus = bookIssueVM.bookStatus,
                bookReturnStatus = Enums.BookReturnStatus.NotYetReturned,
                BookInstanceId = bookIssueVM.BookInstanceId
            };

            //set book Instance's availabililty to borrowed
            var bookInstance = _context.BookInstance.Find(bookIssueVM.BookInstanceId);
            bookInstance!.bookAvailability = Enums.BookAvailability.Borrowed;

            //decrease parent books available amount
            var parentBook = _context.Book.Find(bookInstance.BookId);
            parentBook!.AvailableAmount -= 1;

            await _context.BookIssue.AddAsync(newBookIssue);
            await _context.SaveChangesAsync();
        }

        public async Task<BookIssueDropdownsVM> GetBookIssueDropdownsValues()
        {
            var response = new BookIssueDropdownsVM()
            {
                BookInstances = await _context.BookInstance.OrderBy(n => n.Id).ToListAsync()
            };

            return response;
        }

        public async Task ReturnBookAsync(BookIssue bookIssue)
        {
            var issue = await _context.BookIssue.FirstOrDefaultAsync(n => n.Id == bookIssue.Id);

            //Find parent book, Instance & user
            var newBookInstance = _context.BookInstance.Find(bookIssue.BookInstanceId);
            var parentBook = _context.Book.Find(newBookInstance!.BookId);
            var user = await _context.ApplicationUsers.Include(c => c.LibraryCard).FirstOrDefaultAsync(u => u.UserName == bookIssue.BorrowersName);

            if (issue != null)
            {
                //check the returned date
                DateTime due = issue.DueDate;
                DateTime returned = bookIssue.TimeReturned;
                bool Greater = GetMostRecentDate(returned, due);
                if (Greater)
                {
                    //pays fine
                    user!.LibraryCard!.Balance += 2.50m;
                }

                issue.ReturningLibrariansName = bookIssue.ReturningLibrariansName;
                issue.TimeReturned = bookIssue.TimeReturned;
                issue.bookReturnStatus = bookIssue.bookReturnStatus;

                //set the availability of the book Instance
                //if book was damaged or lost, user must pay a fee
                if (bookIssue.bookReturnStatus == BookReturnStatus.Lost) {
                    newBookInstance!.bookAvailability = BookAvailability.Unavailable;
                    user!.LibraryCard!.Balance += parentBook!.Price;
                }
                else if (bookIssue.bookReturnStatus == BookReturnStatus.ServereDamage) {
                    newBookInstance!.bookAvailability = BookAvailability.ReadOnly;
                    user!.LibraryCard!.Balance += (parentBook!.Price * 0.25m);
                } else {
                    newBookInstance!.bookAvailability = BookAvailability.Available;
                    //increase parent books availability amount
                    parentBook!.AvailableAmount += 1;
                }

                await _context.SaveChangesAsync();
            }
        }

        public bool GetMostRecentDate(DateTime date1, DateTime date2)
        {
            int result = DateTime.Compare(date1, date2);
            if (result > 0)
            {
                // date1 is later than date2
                return true;
            }
            else
            {
                // date2 is later than date1 or equal
                return false;
            }
        }

        public async Task UpdateBookIssueAsync(EditBookIssueVM editBookIssueVM)
        {
            var issue = await _context.BookIssue.FirstOrDefaultAsync(n => n.Id == editBookIssueVM.Id);

            if (issue != null)
            {
                issue.BorrowersName = editBookIssueVM.BorrowersName;
                issue.BorrowingLibrariansName = editBookIssueVM.BorrowersName;
                issue.ReturningLibrariansName = editBookIssueVM.ReturningLibrariansName;
                issue.TimeBorrowed = editBookIssueVM.TimeBorrowed;
                issue.TimeReturned = editBookIssueVM.TimeReturned;
                issue.DueDate = editBookIssueVM.DueDate;
                issue.bookStatus = editBookIssueVM.bookStatus;
                issue.bookReturnStatus = editBookIssueVM.bookReturnStatus;
                issue.BookInstanceId = editBookIssueVM.BookInstanceId;

                await _context.SaveChangesAsync();
            }
        }
    }
}
