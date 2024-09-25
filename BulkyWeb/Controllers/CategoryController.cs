using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Controllers
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
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder can not exactly match the Name");
            }
            if(ModelState.IsValid)
            {
            _db.Categories.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
            }
            return View(obj);
                     
        }

        //Edit Page
        // Automatically for get request
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            Category? categoryFromDb = _db.Categories.Find(id);

            // Other aproach to find for updata 
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.CategoryId == id);
            //Category? categoryFromDb2 = _db.Categories.Where(u=>u.CategoryId == id).FirstOrDefault();
                         
            if (categoryFromDb == null)
                return NotFound();

           return View(categoryFromDb);

        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
           
            var categoryFromDb = _db.Categories.FirstOrDefault(c => c.CategoryId == obj.CategoryId);
            if (categoryFromDb != null)
            {
                categoryFromDb.Name = obj.Name;
                categoryFromDb.DisplayOrder = obj.DisplayOrder;
                _db.Entry(categoryFromDb).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // Delete 

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            Category? categoryFromDb = _db.Categories.Find(id);

            if (categoryFromDb == null)
                return NotFound();

            return View(categoryFromDb);

        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _db.Categories.Find(id);
            if (obj == null)
                return NotFound();

            _db.Categories.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
          
               

    }
}
