using LibraryManagementApp.Data.Base;
using LibraryManagementApp.Data.ViewModels;
using LibraryManagementApp.Enums;
using LibraryManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApp.Data.Services
{
    public class PublishersService : EntityBaseRepository<Publisher>, IPublishersService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;
        public PublishersService(ApplicationDbContext context, IFileService fileService) : base(context)
        {
            _context = context;
            this._fileService = fileService;
        }
        public static string directoryName = "Images\\Publishers\\";

        public async Task AddNewPublisherAsync(NewPublisherVM data)
        {
            string imageName = "";
            if (data.PublisherLogo != null)
            {
                var result = _fileService.SaveImage(data.PublisherLogo, directoryName);
                if (result.Item1 == 1)
                    imageName = result.Item2;
            }

            var newPublisher = new Publisher()
            {
                PublisherLogo = imageName,
                PublisherName = data.PublisherName,
                PublisherDescription = data.PublisherDescription,
                Rating = data.Rating
            };

            await _context.Publisher.AddAsync(newPublisher);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePublisherAsync(NewPublisherVM data)
        {
            var dbPublisher = await _context.Publisher.FirstOrDefaultAsync(n => n.Id == data.Id);

            string NewImageName = "";
            if (data.PublisherLogo != null)
            {
                //save the publishers new image into the directory
                var result = _fileService.SaveImage(data.PublisherLogo, directoryName);
                if (result.Item1 == 1)
                {
                    //assign the actual new image's name to the string "NewImageName"
                    NewImageName = result.Item2;

                    //delete the publishers old image
                    var oldImage = dbPublisher?.PublisherLogo;
                    if (oldImage != null)
                    {
                        var deleteResult = _fileService.DeleteImage(oldImage, directoryName);
                    }
                }
            }

            if (dbPublisher != null)
            {
                if (data.PublisherLogo != null) { dbPublisher.PublisherLogo = NewImageName; }
                dbPublisher.PublisherName = data.PublisherName;
                dbPublisher.PublisherDescription = data.PublisherDescription;
                dbPublisher.Rating = data.Rating;
                await _context.SaveChangesAsync();
            }
            await _context.SaveChangesAsync();
        }
    }
}
