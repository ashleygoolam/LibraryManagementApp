﻿using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApp.Data.ViewModels
{
    public class NewAuthorVM
    {
        public int Id { get; set; }

        public IFormFile? ProfilePicture { get; set; }
        public string? PreviewPicture { get; set; }

        [Required(ErrorMessage = "The Author's fullname is should be provided!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The Fullname must be between 3 and 60 characters")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "The Biography is required!")]
        public string? Biography { get; set; }
    }
}
