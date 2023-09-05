using Microsoft.EntityFrameworkCore;
using LibraryManagementApp.Data;
using LibraryManagementApp.Data.Services;
using LibraryManagementApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using LibraryManagementApp.Data.Cart;

var builder = WebApplication.CreateBuilder(args);

//DbContext configuaration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("connection string 'DefaultConnection' not found!");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

//Services configuaration
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAuthorsService, AuthorsService>();
builder.Services.AddScoped<IPublishersService, PublishersService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookInstanceService, BookInstanceService>();
builder.Services.AddScoped<IBookIssueService, BookIssueService>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IManageAccountService, ManageAccountService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped(b => Busket.GetBusket(b));

//Authentication and Authorization
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
});

//Controllers
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

//Seed database
AppDbInitializer.Seed(app);
AppDbInitializer.SeedUsersAndRolesAsync(app).Wait();

app.Run();
