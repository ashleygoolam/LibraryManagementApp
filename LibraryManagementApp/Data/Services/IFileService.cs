namespace LibraryManagementApp.Data.Services
{
    public interface IFileService
    {
        Tuple<int, string> SaveImage(IFormFile imageFile, string directoryName);
        public bool DeleteImage(string imageFileName, string directoryName);
    }
}
