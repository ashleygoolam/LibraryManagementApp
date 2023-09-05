using LibraryManagementApp.Data.Services;
using LibraryManagementApp.Data.Static;
using LibraryManagementApp.Data.ViewModels;
using LibraryManagementApp.Enums;
using LibraryManagementApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryManagementApp.Controllers
{
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Librarian)]
    public class BookInstanceController : Controller
    {
        private readonly IBookInstanceService _service;

        public BookInstanceController(IBookInstanceService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int? i, string? searchString)
        {
            if (i < 1) i = 1;
            var allData = await _service.GetAllAsync(n => n.Book);

            if (searchString != null)
            {
                var filteredData = allData.Where(n => n.Book!.Title!.Equals(searchString));
                var filterPagedData = await PaginatedList<BookInstance>.CreateAsync(filteredData, i ?? 1, 8);
                ViewBag.searcheditem = searchString;

                return View(filterPagedData);
            }

            var paginatedData = await PaginatedList<BookInstance>.CreateAsync(allData, i ?? 1, 8);

            int totalPages = PageLength.Length;
            if (i > totalPages) i = totalPages;

            return View(paginatedData);
        }

        //json get the instances that are labled "Availabe"
        public async Task<JsonResult> GetInstance(int bookId)
        {
            var allInstances = await _service.GetAllAsync();
            var results = allInstances.Where(i => i.BookId.Equals(bookId) && i.bookAvailability.Equals(
                                            BookAvailability.Available))
                                         .Select(i => new { id = i.Id, number = i.IsbnNumber })
                                         .ToList();

            return Json(results);
        }

        //Create
        public async Task<IActionResult> Create()
        {
            var bookInstanceDropDownsData = await _service.GetBookInstanceDropDownsValues();

            ViewBag.Books = new SelectList(bookInstanceDropDownsData.Books, "Id", "Title");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BookInstance bookInstance) 
        {
            if (!ModelState.IsValid)
            {
                var bookInstanceDropDownsData = await _service.GetBookInstanceDropDownsValues();
                ViewBag.Books = new SelectList(bookInstanceDropDownsData.Books, "Id", "Title");
                return View(bookInstance);
            }

            await _service.AddNewBookInstanceAsync(bookInstance);
            return RedirectToAction(nameof(Index));
        }

        //Edit
        public async Task<IActionResult> Edit(int id)
        {
            var instanceDetails = await _service.GetByIdAsync(id);
            if (instanceDetails == null)
                return NotFound();

            var response = new BookInstance()
            {
                IsbnNumber = instanceDetails.IsbnNumber,
                bookStatus = instanceDetails.bookStatus,
                BookId = instanceDetails.BookId
            };
            var bookInstanceDropDownsData = await _service.GetBookInstanceDropDownsValues();
            ViewBag.Books = new SelectList(bookInstanceDropDownsData.Books, "Id", "Title");

            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, BookInstance bookInstance)
        {
            if (id != bookInstance.Id) return View("Not Found");

            if (ModelState.IsValid)
            {
                await _service.UpdateBookInstanceAsync(bookInstance);
                return RedirectToAction(nameof(Index));
            }
            var bookInstanceDropDownsData = await _service.GetBookInstanceDropDownsValues();
            ViewBag.Books = new SelectList(bookInstanceDropDownsData.Books, "Id", "Title");

            return View(bookInstance);
        }

        //Details
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var bookInstanceDetails = await _service.GetByIdAsync(id, n => n.Book);
            if (bookInstanceDetails == null)
                return NotFound();
            return View(bookInstanceDetails);
        }

        //Delete
        public async Task<IActionResult> Delete(int id)
        {
            var bookInstanceDetails = await _service.GetByIdAsync(id, n => n.Book);
            if (bookInstanceDetails == null) return View("Not Found");
            return View(bookInstanceDetails);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, BookInstance bookInstance)
        {
            var bookInstanceDetails = await _service.GetByIdAsync(id);
            if (bookInstanceDetails == null) return View("Not Found");

            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
