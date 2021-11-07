using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TrainingCourseApp.Models;
using TrainingCourseApp.Roles;

namespace TrainingCourseApp.Controllers
{
    [Authorize(Roles = Role.Trainer)]
    public class TrainerController : Controller
    {
        // GET: Trainer

        private ApplicationDbContext _context;
        public TrainerController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var userInDb = _context.Trainers
                .SingleOrDefault(t => t.TrainerId == userId);
            return View(userInDb);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var trainerInDb = _context.Trainers
                .SingleOrDefault(t => t.Id == id);
            if (trainerInDb == null)
            {
                return HttpNotFound();
            }
            return View(trainerInDb);
        }

        [HttpPost]
        public ActionResult Edit(Trainer trainer)
        {
            var trainerInDb = _context.Trainers
                .SingleOrDefault(t => t.Id == trainer.Id);
            if (trainerInDb == null)
            {
                return HttpNotFound();
            }
            trainerInDb.Name = trainer.Name;
            trainerInDb.Age = trainer.Age;
            trainerInDb.Address = trainer.Address;
            trainerInDb.Specialty = trainer.Specialty;

            _context.SaveChanges();
            return RedirectToAction("index", "Trainer");
        }

        [HttpGet]
        public ActionResult GetCourses()
        {
            var trainerId = User.Identity.GetUserId();
            var trainer = _context.Trainers.ToList();

            var course = _context.Courses
                .Include(t => t.Category)
                .ToList();
            var courses = _context.TrainerCourses
                .Where(t => t.Trainer.TrainerId == trainerId)
                .Select(t => t.Course)
                .ToList();
            return View(courses);
        }

        [HttpGet]
        public ActionResult TraineesInCourse(int id)
        {
            var traineeCourse = _context.TraineeCourses
                .Where(t => t.CourseId == id)
                .Select(t => t.Trainee)
                .ToList();
            return View(traineeCourse);
        }
    }
}