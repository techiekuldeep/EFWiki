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
            List<Book> objList = _db.Books.Include(u=>u.Publisher).ToList();
            //foreach (var obj in objList)
            //{
                //Without Explicit Loading - Each call creates database round trip - least efficient
                //obj.Publisher = _db.Publishers.Find(obj.Publisher_Id);
                //With Explicit Loading - more efficient
            //    _db.Entry(obj).Reference(u => u.Publisher).Load();
            //}
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
        public async Task<IActionResult> Playground()
        {
            var bookTemp = _db.Books.FirstOrDefault();
            bookTemp.Price = 100;

            var bookCollection = _db.Books;
            decimal totalPrice = 0;

            foreach (var book in bookCollection)
            {
                totalPrice += book.Price;
            }

            var bookList = _db.Books.ToList();
            foreach (var book in bookList)
            {
                totalPrice += book.Price;
            }

            var bookCollection2 = _db.Books;
            var bookCount1 = bookCollection2.Count();

            var bookCount2 = _db.Books.Count();
            return RedirectToAction(nameof(Index));
        }

    }
}
