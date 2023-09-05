using LibraryManagementApp.Models;
using LibraryManagementApp.Enums;
using LibraryManagementApp.Data.Static;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagementApp.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope()) 
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.EnsureCreated();

                //Publishers
                if (!context.Publisher.Any())
                {
                    context.Publisher.AddRange(new List<Publisher>()
                    {
                        new Publisher()
                        {
                            PublisherName = "Publisher 1",
                            PublisherLogo = "http://dotnethow.net/images/cinemas/cinema-1.jpeg",
                            PublisherDescription = "This is the description of the first cinema",
                            Rating = Rating.Good
                        },
                        new Publisher()
                        {
                            PublisherName = "Publisher 2",
                            PublisherLogo = "http://dotnethow.net/images/cinemas/cinema-2.jpeg",
                            PublisherDescription = "This is the description of the first cinema",
                            Rating = Rating.Exellent
                        },
                    });
                    context.SaveChanges();
                }

                //Authors
                if (!context.Author.Any())
                {
                    context.Author.AddRange(new List<Author>()
                    {
                        new Author()
                        {
                            FullName = "Author 1",
                            Biography = "This is the Bio of the first actor",
                            ProfilePicture = "http://dotnethow.net/images/actors/actor-1.jpeg"

                        },
                        new Author()
                        {
                            FullName = "Author 2",
                            Biography = "This is the Bio of the second actor",
                            ProfilePicture = "http://dotnethow.net/images/actors/actor-2.jpeg"
                        },
                    });
                    context.SaveChanges();
                }

                //Books
                if (!context.Book.Any())
                {
                    context.Book.AddRange(new List<Book>()
                    {
                        new Book()
                        {
                            Title = "Life",
                            bookCategory = BookCategory.SelfHelp,
                            Description = "This is the Life book description",
                            DateCreated = DateTime.Now,
                            AvailableAmount = 1,
                            TotalAmount = 2,
                            Price = 19,
                            Location = "Second Floor, Col 3, Row 5, Section 4",
                            PublisherId = 1,
                            Image = "http://dotnethow.net/images/movies/movie-3.jpeg"
                        },
                        new Book()
                        {
                            Title = "The Shawshank Redemption",
                            bookCategory = BookCategory.Fiction,
                            Description = "This is the Shawshank Redemption description",
                            DateCreated = DateTime.Now,
                            AvailableAmount = 2,
                            TotalAmount = 3,
                            Price = 25,
                            Location = "First Floor, Col 1, Row 1, Section 3",
                            PublisherId = 2,
                            Image = "http://dotnethow.net/images/movies/movie-1.jpeg",
                        },
                    });
                    context.SaveChanges();
                }

                //Authors & Books 
                if (!context.Author_Book.Any())
                {
                    context.Author_Book.AddRange(new List<Author_Book>()
                    {
                        //authors of book 1
                        new Author_Book()
                        {
                            AuthorId = 1,
                            BookId = 1
                        },

                        //authors of book 2
                        new Author_Book()
                        {
                            AuthorId = 2,
                            BookId = 2
                        },
                    });
                    context.SaveChanges();
                }

                //BookInstance
                if (!context.BookInstance.Any())
                {
                    context.BookInstance.AddRange(new List<BookInstance>()
                    {
                        //BookInstance of book 1
                        new BookInstance()
                        {
                            IsbnNumber = "978-3-16-148419-1",
                            bookAvailability = BookAvailability.Borrowed,
                            bookStatus = BookStatus.Good,
                            BookId = 1,
                        },
                        new BookInstance()
                        {
                            IsbnNumber = "978-3-16-148419-2",
                            bookAvailability = BookAvailability.Available,
                            bookStatus = BookStatus.New,
                            BookId = 1
                        },

                        //BookInstance of book 2
                        new BookInstance()
                        {
                            IsbnNumber = "730-1-21-419148-1",
                            bookAvailability = BookAvailability.ReadOnly,
                            bookStatus = BookStatus.New,
                            BookId = 2,
                        },
                        new BookInstance()
                        {
                            IsbnNumber = "730-1-21-419148-2",
                            bookAvailability = BookAvailability.ReadOnly,
                            bookStatus = BookStatus.Damaged,
                            BookId = 2
                        },
                        new BookInstance()
                        {
                            IsbnNumber = "730-1-21-419148-3",
                            bookAvailability = BookAvailability.Available,
                            bookStatus = BookStatus.MinimumDamage,
                            BookId = 2
                        },
                    });
                    context.SaveChanges();
                }

                //BookIssue
                if (!context.BookIssue.Any())
                {
                    context.BookIssue.AddRange(new List<BookIssue>()
                    {
                        //currently borrowed
                        new BookIssue()
                        {
                            TimeBorrowed = DateTime.Now,
                            DueDate = DateTime.Now.AddDays(10),
                            bookStatus = BookStatus.Good,
                            BookInstanceId = 1,
                            BorrowersName = "dani",
                            BorrowingLibrariansName = "LibGuy",
                            ReturningLibrariansName = "LibGuy"
                        },

                        //was returned
                        new BookIssue()
                        {
                            TimeBorrowed = DateTime.Now,
                            TimeReturned = DateTime.Now.AddDays(9),
                            DueDate = DateTime.Now.AddDays(10),
                            bookStatus = BookStatus.Good,
                            bookReturnStatus = BookReturnStatus.GoodCondition,
                            BookInstanceId = 2
                        },
                    });
                    context.SaveChanges();
                }
            }
        }

        //Seending the database with the Users and their Roles
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Creating Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.Librarian))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Librarian));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Creating Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                //Admin
                string adminUserEmail = "admin@etickets.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FullName = "Admin User",
                        UserName = "admin-user",
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }
                //Librarian
                string librarianUserEmail = "librarian@etickets.com";

                var librarianUser = await userManager.FindByEmailAsync(librarianUserEmail);
                if (librarianUser == null)
                {
                    var newlibrarianUser = new ApplicationUser()
                    {
                        FullName = "Librarian User",
                        UserName = "librarian-user",
                        Email = librarianUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newlibrarianUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newlibrarianUser, UserRoles.Librarian);
                }
                //User
                string appUserEmail = "user@etickets.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new ApplicationUser()
                    {
                        FullName = "Application User",
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
