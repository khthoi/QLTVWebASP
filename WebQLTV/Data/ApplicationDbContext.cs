using System.Security.Policy;
using Microsoft.EntityFrameworkCore;
using WebQLTV.Models;


namespace WebQLTV.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Ánh xạ các DbSet (bảng)
        public DbSet<Account> User { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookType> BookTypes { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookPublisher> Publishers { get; set; }
        public DbSet<BookBorrow> BookBorrow { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ánh xạ đúng tên bảng trong cơ sở dữ liệu nếu cần (ví dụ: "book")
            modelBuilder.Entity<Book>().ToTable("book");
            modelBuilder.Entity<BookType>().ToTable("type");
            modelBuilder.Entity<Author>().ToTable("author");
            modelBuilder.Entity<BookPublisher>().ToTable("publisher");
            modelBuilder.Entity<Account>().ToTable("user");
            modelBuilder.Entity<Role>().ToTable("role");
            modelBuilder.Entity<BookBorrow>().ToTable("bookborrow");

            modelBuilder.Entity<BookBorrowViewModel>().HasNoKey();
        }
    }
}
