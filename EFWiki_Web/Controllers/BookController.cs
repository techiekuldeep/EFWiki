﻿using Microsoft.AspNetCore.Mvc;
using EFWiki_DataAccess.Data;
using EFWiki_Model.Models;
using System.Collections.Generic;
using EFWiki_Model.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EFWiki_Web.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;
        public BookController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Book> objList = _db.Books.ToList(); 
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

    }
}
