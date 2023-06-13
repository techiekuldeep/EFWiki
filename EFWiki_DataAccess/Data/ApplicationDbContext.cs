using Microsoft.EntityFrameworkCore;
using EFWiki_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFWiki_DataAccess.FluentConfig;
using Microsoft.Extensions.Logging;
namespace EFWiki_DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<MainBookDetails> MainBookDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<BookDetail> BookDetails { get; set; }
        public DbSet<BookAuthorMap> BookAuthorMaps { get; set; }
        //rename to Fluent_BookDetails
        public DbSet<Fluent_BookDetail>BookDetail_Fluent { get; set; }
        public DbSet<Fluent_Book> Fluent_Books { get; set; }
        public DbSet<Fluent_Author> Fluent_Authors { get; set; }
        public DbSet<Fluent_Publisher> Fluent_Publishers { get; set; }
        public DbSet<Fluent_BookAuthorMap> Fluent_BookAuthorMaps { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //No Logging
            //optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Database = EFWiki; TrustServerCertificate=True; Trusted_Connection=True;");

            //Logging
            //optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Database = EFWiki; TrustServerCertificate=True; Trusted_Connection=True;")
            //    .LogTo(Console.WriteLine, new[] {DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Book>().Property(u => u.Price).HasPrecision(10,5);
            modelBuilder.Entity<BookAuthorMap>().HasKey(u => new { u.Author_Id, u.Book_Id });

            modelBuilder.ApplyConfiguration(new FluentAuthorConfig()); 
            modelBuilder.ApplyConfiguration(new FluentBookAuthorMapConfig());
            modelBuilder.ApplyConfiguration(new FluentBookConfig());
            modelBuilder.ApplyConfiguration(new FluentBookDetailConfig());
            modelBuilder.ApplyConfiguration(new FluentPublisherConfig());

            modelBuilder.Entity<Book>().HasData(
               new Book { BookId = 1, Title = "Born to run", ISBN = "123B12", Price = 10.99m, Publisher_Id = 1 },
               new Book { BookId = 2, Title = "Running with the Kenyans", ISBN = "12123B12", Price = 11.99m, Publisher_Id = 2 }
           );

            var bookList = new Book[]
            {
                new Book { BookId = 3, Title = "Eat and Run", ISBN = "77652", Price = 20.99m,Publisher_Id = 3},
                new Book { BookId = 4, Title = "Choosing to Run: A Memoir", ISBN = "CC12B12", Price = 25.99m,Publisher_Id = 1},
                new Book { BookId = 5, Title = "Ten Steps to Becoming a Champion", ISBN = "90392B33", Price = 40.99m,Publisher_Id = 3 }
            };
            modelBuilder.Entity<Book>().HasData(bookList);

            modelBuilder.Entity<Publisher>().HasData(
              new Publisher { Publisher_Id = 1, Name = "Simon & Schuster", Location= "New York"},
              new Publisher { Publisher_Id = 2, Name = "Ballantine Books", Location = "New York" },
              new Publisher { Publisher_Id = 3, Name = "Mariner Books", Location = "Boston" }
          );

            modelBuilder.Entity<MainBookDetails>().HasNoKey().ToView("GetMainBookDetails");
        }
    }
}
