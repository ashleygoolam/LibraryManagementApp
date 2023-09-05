using LibraryManagementApp.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementApp.Models;
using LibraryManagementApp.Enums;
using LibraryManagementApp.Data.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using LibraryManagementApp.Data.Static;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryManagementApp.Controllers
{
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Librarian)]
    public class BookIssueController : Controller
    {
        private readonly IBookIssueService _service;

        public BookIssueController(IBookIssueService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int? i, string? searchString)
        {
            if (i < 1) i = 1;

            var allIssues = await _service.GetAllAsync(n => n.BookInstance);

            if (string.IsNullOrWhiteSpace(searchString))
            {
                var allBookIssues = await PaginatedList<BookIssue>.CreateAsync(allIssues, i ?? 1, 8);
                return View(allBookIssues);
            }

            var filteredData = allIssues.Where(n => "NotYetReturned".Equals(searchString)
                ? n.bookReturnStatus.Equals(Enum.Parse<BookReturnStatus>(searchString))
                : string.Equals(n.BorrowersName, searchString, StringComparison.CurrentCultureIgnoreCase));

            var filterPagedData = await PaginatedList<BookIssue>.CreateAsync(filteredData, i ?? 1, 8);
            ViewBag.searcheditem = searchString;

            //int totalPages = PageLength.Length;
            //if (i > totalPages) i = totalPages;

            return View(filterPagedData);
        }

        //Create
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BookIssueVM bookIssueVM)
        {
            if (!ModelState.IsValid) { return View(bookIssueVM); }

            //await _service.AddAsync(bookIssue);
            await _service.AddNewBookIssueAsync(bookIssueVM);
            return RedirectToAction(nameof(Index));
        }

        //Edit
        public async Task<IActionResult> Edit(int id)
        {
            var bookIssueDetails = await _service.GetByIdAsync(id, n => n.BookInstance);
            if (bookIssueDetails == null)
                return NotFound();

            var response = new EditBookIssueVM()
            {
                BorrowersName = bookIssueDetails.BorrowersName,
                BorrowingLibrariansName = bookIssueDetails.BorrowingLibrariansName,
                ReturningLibrariansName = bookIssueDetails.ReturningLibrariansName,
                TimeBorrowed = bookIssueDetails.TimeBorrowed,
                TimeReturned = bookIssueDetails.TimeReturned,
                DueDate = bookIssueDetails.DueDate,
                bookStatus = bookIssueDetails.bookStatus,
                bookReturnStatus = bookIssueDetails.bookReturnStatus,
                BookInstanceId = bookIssueDetails.BookInstanceId
            };

            var bookIssueDropdownsData = await _service.GetBookIssueDropdownsValues();
            ViewBag.BookInstances = new SelectList(bookIssueDropdownsData.BookInstances, "Id", "IsbnNumber");

            var date = bookIssueDetails.TimeBorrowed;
            ViewBag.TimeBorrowedDate = date;

            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditBookIssueVM editBookIssueVM)
        {
            if (id != editBookIssueVM.Id) return View("Not Found");

            if (ModelState.IsValid)
            {
                //await _service.UpdateAsync(bookIssue.Id, bookIssue);
                await _service.UpdateBookIssueAsync(editBookIssueVM);
                return RedirectToAction(nameof(Index));
            }

            var bookIssueDropdownsData = await _service.GetBookIssueDropdownsValues();
            ViewBag.BookInstances = new SelectList(bookIssueDropdownsData.BookInstances, "Id", "IsbnNumber");

            var date = editBookIssueVM.TimeBorrowed;
            ViewBag.TimeBorrowedDate = date;

            return View(editBookIssueVM);
        }

        //Details
        //[AllowAnonymous]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var bookIssueDetails = await _service.GetByIdAsync(id, n => n.BookInstance);
            if (bookIssueDetails == null)
                return NotFound();
            return View(bookIssueDetails);
        }

        //Delete
        public async Task<IActionResult> Delete(int id)
        {
            var bookIssueDetails = await _service.GetByIdAsync(id);
            if (bookIssueDetails == null) return View("Not Found");
            return View(bookIssueDetails);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, BookIssue bookIssue)
        {
            var bookIssueDetails = await _service.GetByIdAsync(id);
            if (bookIssueDetails == null) return View("Not Found");

            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        //History
        [AllowAnonymous]
        public async Task<IActionResult> History(int? i, string? searchString)
        {
            string email = User.Identity.Name;
            var bookIssueDetails = await _service.GetAllAsync(n => n.BookInstance);
            var results = bookIssueDetails.Where(n => string.Equals(n.BorrowersName, email, StringComparison.CurrentCultureIgnoreCase)).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredData = results.Where(n => n.bookReturnStatus.Equals(Enum.Parse<BookReturnStatus>(searchString)));

                var filterPagedData = await PaginatedList<BookIssue>.CreateAsync(filteredData, i ?? 1, 8);
                ViewBag.searcheditem = searchString;

                return View(filterPagedData);
            }

            var pagedResults = await PaginatedList<BookIssue>.CreateAsync(results, i ?? 1, 8);

            return View(pagedResults);
        }

        //Dues
        [Authorize]
        public async Task<IActionResult> Dues(int id)
        {
            var bookIssueDetails = await _service.GetByIdAsync(id, n => n.BookInstance);
            if (bookIssueDetails == null)
                return View();

            return View(bookIssueDetails);
        }

        //Return A Book
        public async Task<IActionResult> ReturnBook(int id)
        {
            var bookIssueDetails = await _service.GetByIdAsync(id, n => n.BookInstance);
            if (bookIssueDetails == null)
                return NotFound();

            var response = new BookIssue()
            {
                ReturningLibrariansName = bookIssueDetails.ReturningLibrariansName,
                TimeReturned = bookIssueDetails.TimeReturned,
                bookReturnStatus = bookIssueDetails.bookReturnStatus,
                BookInstanceId = bookIssueDetails.BookInstanceId,
                BookInstance = bookIssueDetails.BookInstance,
                BorrowersName = bookIssueDetails.BorrowersName
            };

            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> ReturnBook(int id, BookIssue bookIssue)
        {
            if (id != bookIssue.Id) return View("Not Found");

            var issue = await _service.GetByIdAsync(bookIssue.Id, n => n.BookInstance);

            if (ModelState.IsValid)
            {
                if (bookIssue.bookReturnStatus == BookReturnStatus.NotYetReturned)
                {
                    TempData["Error"] = "The Return Status of a book cannot be set to 'NotYetReturned'. " +
                        "Please specify the current returned state of the book in the Return Status field.";
                    return View(issue);
                }
                await _service.ReturnBookAsync(bookIssue);
                return RedirectToAction(nameof(Index));
            }

            return View(issue);
        }
    }
}
