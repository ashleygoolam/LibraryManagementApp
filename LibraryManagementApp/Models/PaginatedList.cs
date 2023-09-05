using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApp.Models
{
    public class PageLength
    {
        public static int Length { get; set; }
    }
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageLength.Length = TotalPages;
            this.AddRange(items);
        }

        public bool PreviousPage { get { return (PageIndex > 1); } }

        public bool NextPage { get { return (PageIndex < TotalPages); } }

        public static async Task<PaginatedList<T>> CreateAsync(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
