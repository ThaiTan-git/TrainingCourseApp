using System.Linq;
using System.Web.Mvc;
using TrainingCourseApp.Models;
using TrainingCourseApp.Roles;

namespace TrainingCourseApp.Controllers
{
    [Authorize(Roles = Role.Staff)]
    public class CategoryController : Controller
    {
        // GET: Category
        private ApplicationDbContext _context;
        public CategoryController()
        {
            _context = new ApplicationDbContext();
        }
        [HttpGet]
        public ActionResult Index()
        {
            var category = _context.Categories.ToList();
            return View(category);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var newCategory = new Category()
            {
                Name = category.Name,
                Description = category.Description
            };
            _context.Categories.Add(newCategory);
            _context.SaveChanges();
            return RedirectToAction("Index", "Category");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {

            var categoryInDb = _context.Categories
                .SingleOrDefault(t => t.Id == id);
            if (categoryInDb == null)
            {
                return HttpNotFound();
            }
            return View(categoryInDb);
        }
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            var cateInDb = _context.Categories
                .SingleOrDefault(t => t.Id == category.Id);
            if (cateInDb == null)
            {
                return HttpNotFound();
            }
            cateInDb.Name = category.Name;
            cateInDb.Description = category.Description;
            _context.SaveChanges();
            return RedirectToAction("Index", "Category");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var categoryInDb = _context.Categories
                .SingleOrDefault(t => t.Id == id);
            if (categoryInDb == null)
            {
                return HttpNotFound();
            }
            _context.Categories.Remove(categoryInDb);
            _context.SaveChanges();
            return RedirectToAction("Index", "Category");
        }

    }
}