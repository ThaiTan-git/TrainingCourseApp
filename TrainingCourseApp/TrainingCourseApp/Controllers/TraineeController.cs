using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;
using TrainingCourseApp.Models;

namespace TrainingCourseApp.Controllers
{
    public class TraineeController : Controller
    {
        // GET: Trainee

        private ApplicationDbContext _context;
        public TraineeController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var userInDb = _context.Trainees
                .SingleOrDefault(t => t.TraineeId == userId);
            return View(userInDb);
        }
        [HttpGet]
        public ActionResult GetCourses()
        {
            var userId = User.Identity.GetUserId();
            var category = _context.Categories.ToList();

            var courses = _context.TraineeCourses
                .Where(t => t.Trainee.TraineeId == userId)
                .Select(t => t.Course)
                .ToList();
            return View(courses);
        }

        [HttpGet]
        public ActionResult SameCourseTrainee(int id)
        {
            var traineeInDb = _context.TraineeCourses
                .Where(t => t.CourseId == id)
                .Select(t => t.Trainee)
                .ToList();
            return View(traineeInDb);
        }
    }
}