using LibraryManagementApp.Data.ViewModels;
using LibraryManagementApp.Data.Services;
using LibraryManagementApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using LibraryManagementApp.Data.Static;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using LibraryManagementApp.Data;
using System.Drawing.Printing;
using static System.Reflection.Metadata.BlobBuilder;
using LibraryManagementApp.Enums;

namespace LibraryManagementApp.Controllers
{
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Librarian)]
    public class BookController : Controller
    {
        private readonly IBookService _service;

        public BookController(IBookService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int? i)
        {
            if (i < 1) i = 1;

            //var allBook = await _context.Book.Include(n => n.Publisher).OrderBy(n => n.Title).ToListAsync();
            var allBooks = await _service.GetAllAsync();
            var paginatedBookList = await PaginatedList<Book>.CreateAsync(allBooks, i ?? 1, 4);

            int totalPages = PageLength.Length;
            if (i > totalPages) i = totalPages;

            return View(paginatedBookList);
        }

        //Autocomplete for book search
        [AllowAnonymous]
        [HttpPost]
        public async Task<JsonResult> GetTitle(string term)
        {
            var allBooks = await _service.GetAllAsync();
            var results = allBooks.Where(i => i.Title!.ToLower().Contains(term.ToLower()))
                                         .Select(i => new { id = i.Id, name = i.Title })
                                         .ToList();

            return Json(results);
        }

        //Filter
        [AllowAnonymous]
        public async Task<IActionResult> Filter(string searchString)
        {
            var allBooks = await _service.GetAllAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                //var filteredResult = allBooks.Where(n => n.Title!.ToLower().Contains(searchString.ToLower()) ||
                  //n.Description!.ToLower().Contains(searchString.ToLower())).ToList();
                var filteredResultNew = allBooks.Where(n => string.Equals(n.Title, searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();
                //|| string.Equals(n.Description, searchString, StringComparison.CurrentCultureIgnoreCase))

                //var simpleResult = allBooks.Where(n => n.Title!.ToLower().Contains(searchString.ToLower()));
                //var allBook2 = await PaginatedList<Book>.CreateAsync(simpleResult, i, 4);

                if(filteredResultNew.Count() > 0)
                {
                    ViewBag.Message = "Search results for \" " + searchString + " \" ...";
                    return View("SearchResults", filteredResultNew);
                }
                
                ViewBag.Message = "No results for ' " + searchString + " ' ...\n" +
                    "perhaps you could use the auto complete field to help you fill out the search bar if your results came up.";
                return View("SearchResults", filteredResultNew);
            }

            return RedirectToAction(nameof(Index));
        }

        //Details
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id) 
        {
            var bookDetails = await _service.GetBookByIdAsync(id);
            if (bookDetails == null)
                return NotFound();

            return View(bookDetails);
        }

        //Create
        public async Task<IActionResult> Create()
        {
            var bookDropdownsData = await _service.GetNewBookDropdownsValues();

            ViewBag.Publishers = new SelectList(bookDropdownsData.Publishers, "Id", "PublisherName");
            ViewBag.Authors = new MultiSelectList(bookDropdownsData.Authors, "Id", "FullName");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(NewBookVM book)
        {
            if (!ModelState.IsValid)
            {
                var bookDropdownsData = await _service.GetNewBookDropdownsValues();

                ViewBag.Publishers = new SelectList(bookDropdownsData.Publishers, "Id", "PublisherName");
                ViewBag.Authors = new SelectList(bookDropdownsData.Authors, "Id", "FullName");

                return View(book);
            }

            if (book.Price < 0m)
            {
                TempData["Error"] = "The Price of a book cannot be negative!";

                var bookDropdownsData = await _service.GetNewBookDropdownsValues();
                ViewBag.Publishers = new SelectList(bookDropdownsData.Publishers, "Id", "PublisherName");
                ViewBag.Authors = new SelectList(bookDropdownsData.Authors, "Id", "FullName");
                return View(book);
            }
            await _service.AddNewBookAsync(book);
            return RedirectToAction(nameof(Index));
        }

        //Edit
        public async Task<IActionResult> Edit(int id)
        {
            var bookDetails = await _service.GetBookByIdAsync(id);
            if (bookDetails == null)
                return NotFound();

            var response = new NewBookVM()
            {
                Title = bookDetails.Title,
                bookCategory = bookDetails.bookCategory,
                Description = bookDetails.Description,
                DateCreated = bookDetails.DateCreated,
                //AvailableAmount = bookDetails.AvailableAmount,
                //TotalAmount = bookDetails.TotalAmount,
                Price = bookDetails.Price,
                Location = bookDetails.Location,
                ImagePreview = bookDetails.Image,
                PublisherId = bookDetails.PublisherId,
                AuthorIds = bookDetails.Author_Book?.Select(n => n.AuthorId).ToList(),
            };

            var bookDropdownsData = await _service.GetNewBookDropdownsValues();

            ViewBag.Publishers = new SelectList(bookDropdownsData.Publishers, "Id", "PublisherName");
            ViewBag.Authors = new SelectList(bookDropdownsData.Authors, "Id", "FullName");

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewBookVM newBookVM)
        {
            if (id != newBookVM.Id) return View("Not Found");

            if (ModelState.IsValid)
            {
                await _service.UpdateBookAsync(newBookVM);
                return RedirectToAction(nameof(Index));
            }

            var bookDropdownsData = await _service.GetNewBookDropdownsValues();

            ViewBag.Publishers = new SelectList(bookDropdownsData.Publishers, "Id", "PublisherName");
            ViewBag.Authors = new SelectList(bookDropdownsData.Authors, "Id", "FullName");

            return View(newBookVM);
        }
    }
}
