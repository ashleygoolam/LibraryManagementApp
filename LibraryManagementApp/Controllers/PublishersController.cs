using LibraryManagementApp.Data;
using LibraryManagementApp.Data.Services;
using LibraryManagementApp.Data.Static;
using LibraryManagementApp.Data.ViewModels;
using LibraryManagementApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LibraryManagementApp.Controllers
{
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Librarian)]
    public class PublishersController : Controller
    {
        private readonly IPublishersService _service;

        public PublishersController(IPublishersService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var allPublishers = await _service.GetAllAsync();
            return View(allPublishers);
        }

        //Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewPublisherVM publisherVM)
        {
            if (!ModelState.IsValid)
            {
                return View(publisherVM);
            }
            await _service.AddNewPublisherAsync(publisherVM);
            return RedirectToAction(nameof(Index));
        }

        //Details
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var publishersDetails = await _service.GetByIdAsync(id);
            if (publishersDetails == null)
                return NotFound();
            return View(publishersDetails);
        }

        //Edit
        public async Task<IActionResult> Edit(int id)
        {
            var publisherDetails = await _service.GetByIdAsync(id);
            if (publisherDetails == null)
                return NotFound();

            var response = new NewPublisherVM()
            {
                PublisherLogoPreview = publisherDetails.PublisherLogo,
                PublisherName = publisherDetails.PublisherName,
                PublisherDescription = publisherDetails.PublisherDescription,
                Rating = publisherDetails.Rating
            };

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewPublisherVM publisherVM)
        {
            if (id != publisherVM.Id) return View("Not Found");

            if (ModelState.IsValid)
            {
                await _service.UpdatePublisherAsync(publisherVM);
                return RedirectToAction(nameof(Index));
            }
            return View(publisherVM);
        }

        //Delete
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var publisherDetails = await _service.GetByIdAsync(id);
            if (publisherDetails == null) return View("Not Found");
            return View(publisherDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, [Bind("Id, FullName, ProfilePicture, Biography")] Publisher publisher)
        {
            var publisherDetails = await _service.GetByIdAsync(id);
            if (publisherDetails == null) return View("Not Found");

            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
