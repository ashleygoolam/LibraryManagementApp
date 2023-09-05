using LibraryManagementApp.Data.Base;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? ProfilePicture { get; set; }

        //Relationships
        public LibraryCard? LibraryCard { get; set; }
    }
}
