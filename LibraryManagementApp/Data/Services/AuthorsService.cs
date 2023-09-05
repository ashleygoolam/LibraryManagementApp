using LibraryManagementApp.Data.Base;
using LibraryManagementApp.Data.ViewModels;
using LibraryManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApp.Data.Services
{
    public class AuthorsService : EntityBaseRepository<Author>, IAuthorsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;
        public AuthorsService(ApplicationDbContext context, IFileService fileService) : base(context) 
        {
            _context = context;
            this._fileService = fileService;
        }
        public static string directoryName = "Images\\Authors\\";

        public async Task AddNewAuthorAsync(NewAuthorVM data)
        {
            string imageName = "";
            if (data.ProfilePicture != null)
            {
                var result = _fileService.SaveImage(data.ProfilePicture, directoryName);
                if (result.Item1 == 1)
                    imageName = result.Item2;
            }

            var newAuthor = new Author()
            {
                ProfilePicture = imageName,
                FullName = data.FullName,
                Biography = data.Biography
            };

            await _context.Author.AddAsync(newAuthor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAuthorAsync(NewAuthorVM data)
        {
            var dbAuthor = await _context.Author.FirstOrDefaultAsync(n => n.Id == data.Id);

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
                    var oldImage = dbAuthor?.ProfilePicture;
                    if (oldImage != null)
                    {
                        var deleteResult = _fileService.DeleteImage(oldImage, directoryName);
                    }
                }
            }

            if (dbAuthor != null)
            {
                if(data.ProfilePicture != null) { dbAuthor.ProfilePicture = NewImageName; }
                dbAuthor.FullName = data.FullName;
                dbAuthor.Biography = data.Biography;
                await _context.SaveChangesAsync();
            }
            await _context.SaveChangesAsync();
        }
    }
}
