// See https://aka.ms/new-console-template for more information
using EFWiki_DataAccess.Data;

using EFWiki_Model.Models;

using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");
//Datatbase Helper Methods
//using (ApplicationDbContext context = new())
//{
//    context.Database.EnsureCreated();
//    if (context.Database.GetPendingMigrations().Count() > 0)
//    {
//        context.Database.Migrate();
//    }
//}

//AddBook();
//GetAllBooks();
//GetBook();
//UpdateBook();
//DeleteBook();

//void DeleteBook()
//{
//    using var context = new ApplicationDbContext();
//    var book = context.Books.Find(7);
//    context.Books.Remove(book);
//    context.SaveChanges();
//}
//async void DeleteBook()
//{
//    using var context = new ApplicationDbContext();
//    var book = await context.Books.FindAsync(7);
//    context.Books.Remove(book);
//    await context.SaveChangesAsync();
//}

//void UpdateBook()
//{
//    try
//    {
//        using var context = new ApplicationDbContext();
//        var books = context.Books.Where(u => u.Publisher_Id == 1);
//        foreach (var book in books)
//        {
//            book.Price = 28.50m;
//        }
//        //Console.WriteLine(book.Title + " - " + book.ISBN);
//        context.SaveChanges();
//    }
//    catch (Exception e)
//    {

//    }
//}

//async void UpdateBook()
//{
//    try
//    {
//        using var context = new ApplicationDbContext();
//        var books = await context.Books.Where(u => u.Publisher_Id == 1).ToListAsync();
//        foreach (var book in books)
//        {
//            book.Price = 28.50m;
//        }
//        //Console.WriteLine(book.Title + " - " + book.ISBN);
//        await context.SaveChangesAsync();
//    }
//    catch (Exception e)
//    {

//    }
//}

//void GetBook()
//{
//    try
//    {
//        using var context = new ApplicationDbContext();
//        var books = context.Books.Skip(0).Take(2);
//        //Console.WriteLine(book.Title + " - " + book.ISBN);
//        foreach (var book in books)
//        {
//            Console.WriteLine(book.Title + " - " + book.ISBN);
//        }
//        books = context.Books.Skip(4).Take(1);
//        //Console.WriteLine(book.Title + " - " + book.ISBN);
//        foreach (var book in books)
//        {
//            Console.WriteLine(book.Title + " - " + book.ISBN);
//        }
//    }
//    catch (Exception e)
//    {

//    }
//}

//async void GetBook()
//{
//    try
//    {
//        using var context = new ApplicationDbContext();
//        var books = await context.Books.Skip(0).Take(2).ToListAsync();
//        //Console.WriteLine(book.Title + " - " + book.ISBN);
//        foreach (var book in books)
//        {
//            Console.WriteLine(book.Title + " - " + book.ISBN);
//        }
//        books = await context.Books.Skip(4).Take(1).ToListAsync();
//        //Console.WriteLine(book.Title + " - " + book.ISBN);
//        foreach (var book in books)
//        {
//            Console.WriteLine(book.Title + " - " + book.ISBN);
//        }
//    }
//    catch (Exception e)
//    {

//    }
//}

//void GetAllBooks()
//{
//    using var context = new ApplicationDbContext();
//    var books = context.Books.ToList();
//    foreach (var book in books)
//    {
//        Console.WriteLine(book.Title + " - " + book.ISBN );
//    }
//}


//void AddBook()
//{ Book book = new() { Title = "Connect the Dots", ISBN = "999-81-90421-2-1", Price =35.5m, Publisher_Id = 3 };
//    using var context = new ApplicationDbContext();
//    var books = context.Books.Add(book);
//    context.SaveChanges();
//}

//async void AddBook()
//{
//    Book book = new() { Title = "Connect the Dots", ISBN = "999-81-90421-2-1", Price = 35.5m, Publisher_Id = 3 };
//    using var context = new ApplicationDbContext();
//    var books = await context.Books.AddAsync(book);
//    await context.SaveChangesAsync();
//}