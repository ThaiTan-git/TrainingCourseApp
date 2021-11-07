using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrainingCourseApp.Models;
using TrainingCourseApp.Roles;
using TrainingCourseApp.ViewModels;

namespace TrainingCourseApp.Controllers
{
    [Authorize(Roles = Role.Staff)]
    public class StaffController : Controller
    {
        // GET: Staff
        private ApplicationDbContext _context;
        private ApplicationUserManager _userManager;
        public StaffController()
        {
            _context = new ApplicationDbContext();
        }
        public StaffController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
            _context = new ApplicationDbContext();
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateTrainee()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTrainee(CreateTraineeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                var userId = user.Id;

                var newTrainee = new Trainee()
                {
                    TraineeId = userId,
                    Email = model.Email,
                    Name = model.Name,
                    Age = model.Age,
                    DateOfBirth = model.DateOfBirth,
                    Address = model.Address,
                    Education = model.Education
                };
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, Role.Trainee);
                    _context.Trainees.Add(newTrainee);
                    _context.SaveChanges();
                    return RedirectToAction("GetTrainee", "Staff");
                }
                AddErrors(result);
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult EditTrainee(int id)
        {
            var traineeInDb = _context.Trainees
                .SingleOrDefault(t => t.Id == id);
            if (traineeInDb == null)
            {
                return HttpNotFound();
            }
            return View(traineeInDb);
        }

        [HttpPost]
        public ActionResult EditTrainee(Trainee trainee)
        {
            var traineeInDb = _context.Trainees
                .SingleOrDefault(t => t.Id == trainee.Id);
            if (traineeInDb == null)
            {
                ModelState.AddModelError("", "Trainee is invalid");
                return RedirectToAction("GetTrainee", "Staff");
            }
            traineeInDb.Name = trainee.Name;
            traineeInDb.Age = trainee.Age;
            traineeInDb.DateOfBirth = trainee.DateOfBirth;
            traineeInDb.Address = trainee.Address;
            traineeInDb.Education = trainee.Education;

            _context.SaveChanges();
            return RedirectToAction("GetTrainee", "Staff");
        }

        [HttpGet]
        public ActionResult GetTrainee(string searchName)
        {
            var trainee = _context.Trainees.ToList();
            if (!string.IsNullOrEmpty(searchName))
            {
                trainee = trainee
                    .Where(t => t.Name
                    .ToLower()
                    .Contains(searchName.ToLower())
                    || t.Age.ToString()
                    .Contains(searchName.ToLower()))
                    .ToList();
            }
            return View(trainee);
        }

        [HttpGet]
        public ActionResult DeleteTrainee(string id)
        {
            var traineeInDb = _context.Users
                .SingleOrDefault(t => t.Id == id);
            var infoInDb = _context.Trainees
                .SingleOrDefault(t => t.TraineeId == id);

            if (traineeInDb == null || infoInDb == null)
            {
                ModelState.AddModelError("", "Trainee is invalid");
                return RedirectToAction("GetTrainee", "Staff");
            }
            _context.Users.Remove(traineeInDb);
            _context.Trainees.Remove(infoInDb);
            _context.SaveChanges();
            return RedirectToAction("GetTrainee", "Staff");
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}
