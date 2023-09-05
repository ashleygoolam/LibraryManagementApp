using LibraryManagementApp.Data.ViewModels;
using LibraryManagementApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApp.Data.Services
{
    public class ManageAccountService : IManageAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileService _fileService;
        public ManageAccountService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IFileService fileService)
        {
            _context = context;
            _userManager = userManager;
            this._fileService = fileService;
        }

        public static string directoryName = "Images\\UsersProfilePictures\\";

        public async Task UpdateUserAsync(ManageAccountVM data)
        {
            var dbUser = await _context.Users.SingleOrDefaultAsync(n => n.Id == data.Id);

            if (dbUser != null)
            {
                string NewImageName = "";
                if (data.ProfilePicture != null)
                {
                    //save the authors new image into the directory
                    var result = _fileService.SaveImage(data.ProfilePicture, directoryName);
                    if (result.Item1 == 1)
                    {
                        //assign the actual new image's name to the string "NewImageName"
                        NewImageName = result.Item2;

                        //delete the authors old image
                        var oldImage = dbUser?.ProfilePicture;
                        if (oldImage != null)
                        {
                            var deleteResult = _fileService.DeleteImage(oldImage, directoryName);
                        }
                    }
                }
                else
                {
                    NewImageName = dbUser?.ProfilePicture ?? "";
                }
            
                var userDetails = new ApplicationUser()
                {
                    FullName = data.FullName,
                    PhoneNumber = data.PhoneNumber,
                    ProfilePicture = NewImageName
                };

                var IdentityResult = await _userManager.UpdateAsync(userDetails);

                if (IdentityResult.Succeeded)
                {
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
