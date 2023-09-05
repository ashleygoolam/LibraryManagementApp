using LibraryManagementApp.Data;
using LibraryManagementApp.Data.Services;
using LibraryManagementApp.Data.Static;
using LibraryManagementApp.Data.ViewModels;
using LibraryManagementApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LibraryManagementApp.Controllers
{
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Librarian)]
    public class AuthorController : Controller
    {
        private readonly IAuthorsService _service;

        public AuthorController(IAuthorsService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        //Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewAuthorVM newAuthorVM)
        {
            if(!ModelState.IsValid)
            {
                return View(newAuthorVM);
            }

            await _service.AddNewAuthorAsync(newAuthorVM);
            return RedirectToAction(nameof(Index));
        }

        //Details
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var authorDetails = await _service.GetByIdAsync(id);
            if (authorDetails == null)
                return NotFound();
            return View(authorDetails);
        }

        //Edit
        public async Task<IActionResult> Edit(int id)
        {
            var authorDetails = await _service.GetByIdAsync(id);
            if (authorDetails == null)
                return NotFound();

            var response = new NewAuthorVM()
            {
                PreviewPicture = authorDetails.ProfilePicture,
                FullName = authorDetails.FullName,
                Biography = authorDetails.Biography
            };

            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewAuthorVM newAuthorVM)
        {
            if (id != newAuthorVM.Id) return View("Not Found");

            if (ModelState.IsValid)
            {
                await _service.UpdateAuthorAsync(newAuthorVM);
                return RedirectToAction(nameof(Index));
            }
            return View(newAuthorVM);
        }

        //Delete
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var authorDetails = await _service.GetByIdAsync(id);
            if (authorDetails == null) return View("Not Found");
            return View(authorDetails);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, [Bind("Id, FullName, ProfilePicture, Biography")] Author author)
        {
            var authorDetails = await _service.GetByIdAsync(id);
            if (authorDetails == null) return View("Not Found");

            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
