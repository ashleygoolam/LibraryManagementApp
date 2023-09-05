using LibraryManagementApp.Data.Base;
using LibraryManagementApp.Data.ViewModels;
using LibraryManagementApp.Enums;
using LibraryManagementApp.Models;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace LibraryManagementApp.Data.Services
{
    public class BookService : EntityBaseRepository<Book>, IBookService
    {
        private readonly IFileService _fileService;
        private readonly ApplicationDbContext _context;
        public BookService(ApplicationDbContext context, IFileService fileService) : base(context)
        {
            _context = context;
            this._fileService = fileService;
        }
        public static string directoryName = "Images\\Books\\";

        public async Task AddNewBookAsync(NewBookVM data)
        {
            string imageName = "";
            if (data.Image != null)
            {
                var result = _fileService.SaveImage(data.Image, directoryName);
                if(result.Item1 == 1)
                    imageName = result.Item2;
            }

            var newBook = new Book()
            {
                Title = data.Title,
                bookCategory = data.bookCategory,
                Description = data.Description,
                DateCreated = data.DateCreated,
                AvailableAmount = 0,
                TotalAmount = 0,
                Price = data.Price,
                Location = data.Location,
                Image = imageName,
                PublisherId = data.PublisherId
            };
            await _context.Book.AddAsync(newBook);
            await _context.SaveChangesAsync();

            //Add Authors
            foreach (var authorId in data.AuthorIds) 
            {
                var newAuthorBook = new Author_Book()
                {
                    BookId = newBook.Id,
                    AuthorId = authorId
                };
                await _context.Author_Book.AddAsync(newAuthorBook);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            var bookDetails = await _context.Book
                .Include(p => p.Publisher)
                .Include(ab => ab.Author_Book)!.ThenInclude(a => a.Author)
                .FirstOrDefaultAsync(n => n.Id == id);

            return bookDetails;
        }

        public async Task<NewBookDropdownsVM> GetNewBookDropdownsValues()
        {
            var response = new NewBookDropdownsVM()
            {
                Authors = await _context.Author.OrderBy(n => n.FullName).ToListAsync(),
                Publishers = await _context.Publisher.OrderBy(n => n.PublisherName).ToListAsync()
            };

            return response;
        }

        public async Task UpdateBookAsync(NewBookVM data)
        {
            var dbBook = await _context.Book.FirstOrDefaultAsync(n => n.Id == data.Id);

            string NewImageName = "";
            if (data.Image != null)
            {
                //save the books new image into the directory
                var result = _fileService.SaveImage(data.Image, directoryName);
                if (result.Item1 == 1)
                {
                    //assign the actual new image's name to the string "NewImageName"
                    NewImageName = result.Item2;

                    //delete the books old image
                    var oldImage = dbBook?.Image;
                    if (oldImage != null)
                    {
                        var deleteResult = _fileService.DeleteImage(oldImage, directoryName);
                    }
                }
            }

            if (dbBook != null)
            {
                dbBook.Title = data.Title;
                dbBook.bookCategory = data.bookCategory;
                dbBook.Description = data.Description;
                dbBook.DateCreated = data.DateCreated;
                //dbBook.AvailableAmount = data.AvailableAmount;
                //dbBook.TotalAmount = data.TotalAmount;
                dbBook.Price = data.Price;
                dbBook.Location = data.Location;
                if(data.Image != null) { dbBook.Image = NewImageName; }
                dbBook.PublisherId = data.PublisherId;
                await _context.SaveChangesAsync();
            }

            //Remove existing Authors
            var existingAuthorDb = _context.Author_Book.Where(n => n.BookId == data.Id).ToList();
            _context.Author_Book.RemoveRange(existingAuthorDb);
            await _context.SaveChangesAsync();

            //Add Book Authors
            foreach (var authorId in data.AuthorIds) 
            {
                var newAuthorBook = new Author_Book()
                {
                    BookId = data.Id,
                    AuthorId = authorId
                };
                await _context.Author_Book.AddAsync(newAuthorBook);
            }
            await _context.SaveChangesAsync();
        }
    }
}
