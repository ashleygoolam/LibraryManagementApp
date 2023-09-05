using LibraryManagementApp.Data;
using LibraryManagementApp.Data.Services;
using LibraryManagementApp.Data.Static;
using LibraryManagementApp.Data.ViewModels;
using LibraryManagementApp.Models;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace LibraryManagementApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IManageAccountService _manageAccountService;
        private readonly IFileService _fileService;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IFileService fileService,
            IManageAccountService manageAccountService,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _fileService = fileService;
            _manageAccountService = manageAccountService;
        }

        private async Task<InputModel> LoadInputModelAsync(ApplicationUser user)
        {
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            
            //var userCard
            return new InputModel
            {
                PhoneNumber = phoneNumber,
                FullName = user.FullName,
                ProfilePicture = user.ProfilePicture
            };
        }

        [HttpGet]
        public async Task<IActionResult> ManageAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var inputModel = await LoadInputModelAsync(user);

            return View(inputModel);
        }
        [HttpPost]
        public async Task<IActionResult> ManageAccount(InputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Unexpected error when trying to set phone number.");
                    return View(input);
                }
            }
            //Update the Name
            if (input.FullName != user.FullName)
            {
                user.FullName = input.FullName;
                await _userManager.UpdateAsync(user);
            }
            //Update the Profile Picture
            string directoryName = "Images\\UsersProfilePictures\\";
            if (input.ImageFile != null)
            {
                var result = _fileService.SaveImage(input.ImageFile, directoryName);
                if (result.Item1 == 1)
                {
                    var oldImage = user.ProfilePicture;
                    user.ProfilePicture = result.Item2;
                    await _userManager.UpdateAsync(user);
                    var deleteResult = _fileService.DeleteImage(oldImage!, directoryName);
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["StatusMessage"] = "Your profile has been updated";
            return RedirectToAction(nameof(ManageAccount));
        }

        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Users(int? i)
        {
            if (i < 1) i = 1;

            var users = await _context.Users.ToListAsync();
            var paginatedUsers = await PaginatedList<ApplicationUser>.CreateAsync(users, i ?? 1, 8);

            int totalPages = PageLength.Length;
            if (i > totalPages) i = totalPages;

            return View(paginatedUsers);
        }

        //details
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UsersDetails(string? id)
        {
            var user = await _context.ApplicationUsers.Include(c => c.LibraryCard).FirstOrDefaultAsync(u => u.Id == id);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUserRole(string userId, string desiredRole)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return RedirectToAction("Users", "Account");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var currentRole = roles.FirstOrDefault();

            if (currentRole!.Contains(desiredRole))
            {
                // User already has desired role, do nothing
                return RedirectToAction("Users", "Account");
            }

            if (currentRole != desiredRole)
            {
                // User has current role, remove it and assign desired role
                await _userManager.RemoveFromRoleAsync(user, currentRole);
                await _userManager.AddToRoleAsync(user, desiredRole);
            }
            else
            {
                // User doesn't have current role, assign desired role directly
                await _userManager.AddToRoleAsync(user, desiredRole);
            }

            return RedirectToAction("Users", "Account");
        }


        public IActionResult Login() => View(new LoginVM());

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Book");
                    }
                }
                TempData["Error"] = "Wrong credentials. Please, try again!";
                return View(loginVM);
            }

            TempData["Error"] = "Wrong credentials. Please, try again!";
            return View(loginVM);
        }

        public IActionResult Register() => View(new RegisterVM());

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerVM);
            }

            var newUser = new ApplicationUser()
            {
                FullName = registerVM.FullName,
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);

                var libraryCard = new LibraryCard()
                {
                    ApplicationUserId = newUser.Id,
                    Owner = newUser.UserName,
                    CardNumber = Guid.NewGuid(),
                    Balance = 0.00m,
                    isActivated = false
                };

                _context.LibraryCards.Add(libraryCard);
                await _context.SaveChangesAsync();
            }

            return View("RegisterCompleted");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //Autocomplete for the users name
        [HttpPost]
        public async Task<JsonResult> GetBorrower(string term)
        {
            var results = await _context.Users.Where(i => i.UserName!.ToLower().Contains(term.ToLower()))
                                         .Select(i => new { id = i.Id, name = i.UserName })
                                         .ToListAsync();

            return Json(results);
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }
    }
}
