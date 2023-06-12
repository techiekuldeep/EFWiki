using Microsoft.AspNetCore.Mvc;
using EFWiki_DataAccess.Data;
using EFWiki_Model.Models;
using System.Collections.Generic;

namespace EFWiki_Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> objList = _db.Categories.ToList(); 
            return View(objList);
        }

        public IActionResult Upsert(int? id)
        {
            Category obj= new();
            if (id == null || id==0)
            {
                //Create
                return View(obj);
            }
            //edit
            obj = _db.Categories.First(u=>u.CategoryId==id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Category obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.CategoryId == 0)
                {
                    //Create
                    await _db.Categories.AddAsync(obj);
                }
                else 
                {
                    //Update
                    _db.Categories.Update(obj);
                }
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            Category obj = new();
            obj = _db.Categories.First(u => u.CategoryId == id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(obj);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
