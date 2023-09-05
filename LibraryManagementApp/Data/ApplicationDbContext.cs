using LibraryManagementApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Author_Book Relationship
            modelBuilder.Entity<Author_Book>().HasKey(ab => new { ab.AuthorId, ab.BookId });

            modelBuilder.Entity<Author_Book>().HasOne(b => b.Book).WithMany(ab => ab.Author_Book)
                .HasForeignKey(b => b.BookId);
            modelBuilder.Entity<Author_Book>().HasOne(b => b.Author).WithMany(ab => ab.Author_Book)
                .HasForeignKey(b => b.AuthorId);

            //LibraryCard Relationship
            modelBuilder.Entity<LibraryCard>().HasKey(n => n.Id);//pk

            modelBuilder.Entity<LibraryCard>().HasOne(u => u.ApplicationUser)
                .WithOne(c => c.LibraryCard)
                .HasForeignKey<LibraryCard>(fk => fk.ApplicationUserId);

            modelBuilder.Entity<LibraryCard>().Property(l => l.Id).HasColumnType("int");

            //BookIssue Relationships
            //modelBuilder.Entity<BookIssue>().HasKey(n => n.Id);//pk

            //modelBuilder.Entity<BookIssue>().HasOne(b => b.Card).WithMany(c => c.BooksIssued)
            //    .HasForeignKey(b => b.CardId)
            //    .OnDelete(DeleteBehavior.Cascade);
            //modelBuilder.Entity<BookIssue>().HasOne(b => b.BookInstance).WithMany(i => i.BookIssue)
            //    .HasForeignKey(fk => fk.BookInstanceId);

            //Decimal value specifications
            modelBuilder.Entity<Book>().Property(b => b.Price).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<LibraryCard>().Property(l => l.Balance).HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<LibraryCard> LibraryCards { get; set; }
        public DbSet<BookInstance> BookInstance { get; set; }
        public DbSet<BookIssue> BookIssue { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Author_Book> Author_Book { get; set; }
        public DbSet<Publisher> Publisher { get; set; }

        //Order related tables
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<BusketItems> BusketItems { get; set; }
    }
}
