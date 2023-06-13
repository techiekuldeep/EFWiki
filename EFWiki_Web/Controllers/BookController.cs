﻿using Microsoft.AspNetCore.Mvc;
using EFWiki_DataAccess.Data;
using EFWiki_Model.Models;
using System.Collections.Generic;
using EFWiki_Model.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EFWiki_Web.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;
        public BookController(ApplicationDbContext db)
        {
            _db = db;
        }
        //public IActionResult Index()
        //{
        //    List<Book> objList = _db.Books.ToList();
        //    foreach (var obj in objList)
        //    {
        //        //Without Explicit Loading - Each call creates database round trip - least efficient
        //        //obj.Publisher = _db.Publishers.Find(obj.Publisher_Id);
        //        //With Explicit Loading - more efficient
        //        _db.Entry(obj).Reference(u => u.Publisher).Load();
        //    }
        //    return View(objList);
        //}

        //Eager Loading to avoid n+1 exceution
        public IActionResult Index()
        {
            //List<Book> objList = _db.Books.Include(u=>u.Publisher).ToList();
            List<Book> objList = _db.Books.ToList();
            foreach (var obj in objList)
                {
                //Without Explicit Loading - Each call creates database round trip - least efficient
                //obj.Publisher = _db.Publishers.Find(obj.Publisher_Id);
                //With Explicit Loading - more efficient
                _db.Entry(obj).Reference(u => u.Publisher).Load();
                _db.Entry(obj).Collection(u => u.BookAuthorMap).Load();
                foreach (var bookAuth in obj.BookAuthorMap)
                {
                    _db.Entry(bookAuth).Reference(u => u.Author).Load();
                }
            }
            return View(objList);
        }

        public IActionResult Upsert(int? id)
        {
            BookVM obj= new();
            obj.PublishersList = _db.Publishers.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Publisher_Id.ToString()
            });

            if (id == null || id==0)
            {
                //Create
                return View(obj);
            }
            //edit
            obj.Book = _db.Books.FirstOrDefault(u=>u.BookId==id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(BookVM obj)
        {
            
                if (obj.Book.BookId == 0)
                {
                    //Create
                    await _db.Books.AddAsync(obj.Book);
                }
                else 
                {
                //Update
                _db.Books.Update(obj.Book);
                }
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        //public IActionResult Details(int? id)
        //{
        //    BookVM obj = new();
           
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    //edit
        //    obj.Book = _db.Books.FirstOrDefault(u => u.BookId == id);
        //    //one way to update Book Detail using BookVM
        //    obj.Book.BookDetail = _db.BookDetails.FirstOrDefault(u => u.Book_Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(obj);
        //}

        public IActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            BookDetail obj = new();
            //edit
            //obj.Book = _db.Books.FirstOrDefault(u => u.BookId == id);
            //another optimal way to update Book Detail without BookVM
            //obj = _db.BookDetails.FirstOrDefault(u => u.Book_Id == id);
            //Eager Loading
            obj = _db.BookDetails.Include(u => u.Book).FirstOrDefault(u => u.Book_Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(BookDetail obj)
        {
            if (obj.BookDetail_Id== 0)
            {
                //Create
                await _db.BookDetails.AddAsync(obj);
            }
            else
            {
                //Update
                _db.BookDetails.Update(obj);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            Book obj = new();
            obj = _db.Books.FirstOrDefault(u => u.BookId == id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Books.Remove(obj);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ManageAuthors(int id)
        {
            BookAuthorVM obj = new()
            {
                BookAuthorList = _db.BookAuthorMaps.Include(u => u.Author).Include(u => u.Book)
                    .Where(u => u.Book_Id == id).ToList(),
                BookAuthor = new()
                {
                    Book_Id = id
                },
                Book = _db.Books.FirstOrDefault(u => u.BookId == id)
            };

            List<int> tempListOfAssignedAuthor = obj.BookAuthorList.Select(u => u.Author_Id).ToList();

            //NOT IN clause
            //get all the authors whos id is not in tempListOfAssignedAuthors

            var tempList = _db.Authors.Where(u => !tempListOfAssignedAuthor.Contains(u.Author_Id)).ToList();
            obj.AuthorList = tempList.Select(i => new SelectListItem
            {
                Text = i.FullName,
                Value = i.Author_Id.ToString()
            });

            return View(obj);
        }


        [HttpPost]
        public IActionResult ManageAuthors(BookAuthorVM bookAuthorVM)
        {
            if (bookAuthorVM.BookAuthor.Book_Id != 0 && bookAuthorVM.BookAuthor.Author_Id != 0)
            {
                _db.BookAuthorMaps.Add(bookAuthorVM.BookAuthor);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(ManageAuthors), new { @id = bookAuthorVM.BookAuthor.Book_Id });
        }

        [HttpPost]
        public IActionResult RemoveAuthors(int authorId, BookAuthorVM bookAuthorVM)
        {
            int bookId = bookAuthorVM.Book.BookId;
            BookAuthorMap bookAuthorMap = _db.BookAuthorMaps.FirstOrDefault(
                u => u.Author_Id == authorId && u.Book_Id == bookId);


            _db.BookAuthorMaps.Remove(bookAuthorMap);
            _db.SaveChanges();
            return RedirectToAction(nameof(ManageAuthors), new { @id = bookId });
        }
        public async Task<IActionResult> Playground()
        {
            //Deferred Execution
            //var bookTemp = _db.Books.FirstOrDefault();
            //bookTemp.Price = 100;

            //var bookCollection = _db.Books;
            //decimal totalPrice = 0;

            //foreach (var book in bookCollection)
            //{
            //    totalPrice += book.Price;
            //}

            //var bookList = _db.Books.ToList();
            //foreach (var book in bookList)
            //{
            //    totalPrice += book.Price;
            //}

            //var bookCollection2 = _db.Books;
            //var bookCount1 = bookCollection2.Count();

            //var bookCount2 = _db.Books.Count();
            

            //IEnumerable vs IQueryable
            IEnumerable<Book> BookList1 = _db.Books;
            var FilteredBook1 = BookList1.Where(b => b.Price > 20).ToList();

            IQueryable<Book> BookList2 = _db.Books;
            var fileredBook2 = BookList2.Where(b => b.Price > 20).ToList();
            return RedirectToAction(nameof(Index));
        }

    }
}
