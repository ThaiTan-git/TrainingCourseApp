using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrainingCourseApp.Models;
using TrainingCourseApp.Roles;
using TrainingCourseApp.ViewModels;
using static TrainingCourseApp.Controllers.ManageController;

namespace TrainingCourseApp.Controllers
{
    [Authorize(Roles = Role.Admin)]
    public class AdminController : Controller
    {
        // GET: Admin
        private ApplicationDbContext _context;
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        public AdminController()
        {
            _context = new ApplicationDbContext();
        }
        public AdminController(ApplicationUserManager userManager)
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
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateStaff()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateStaff(CreateStaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                var StaffId = user.Id;

                var newStaff = new Staff()
                {
                    Email = model.Email,
                    StaffId = StaffId,
                    Name = model.Name,
                    Age = model.Age,
                    Address = model.Address
                };
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, Role.Staff);
                    _context.Staffs.Add(newStaff);
                    _context.SaveChanges();
                    return RedirectToAction("GetStaffs", "Admin");
                }
                AddErrors(result);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult CreateTrainer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTrainer(CreateTrainerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                var TrainerId = user.Id;

                var newTrainer = new Trainer()
                {
                    TrainerId = TrainerId,
                    Email = model.Email,
                    Name = model.Name,
                    Age = model.Age,
                    Address = model.Address,
                    Specialty = model.Specialty
                };
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, Role.Trainer);
                    _context.Trainers.Add(newTrainer);
                    _context.SaveChanges();
                    return RedirectToAction("GetTrainers", "Admin");
                }
                AddErrors(result);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult GetStaffs(ManageMessageId? message)
        {
            ViewBag.StatusMessage = message == ManageMessageId.ChangePasswordSuccess ? "Password has been changed." : "";
            var staffs = _context.Staffs.ToList();
            return View(staffs);
        }

        [HttpGet]
        public ActionResult EditStaff(int id)
        {
            var staffInDb = _context.Staffs
                .SingleOrDefault(t => t.Id == id);
            if (staffInDb == null)
            {
                return HttpNotFound();
            }
            return View(staffInDb);
        }

        [HttpPost]
        public ActionResult EditStaff(Staff staff)
        {
            var staffInDb = _context.Staffs.SingleOrDefault(t => t.Id == staff.Id);
            if (staffInDb == null)
            {
                return HttpNotFound();
            }
            staffInDb.Name = staff.Name;
            staffInDb.Age = staff.Age;
            staffInDb.Address = staff.Address;

            _context.SaveChanges();
            return RedirectToAction("GetStaffs", "Admin");
        }

        [HttpGet]
        public ActionResult DeleteStaff(string id)
        {
            var staffInDb = _context.Users
                .SingleOrDefault(t => t.Id == id);
            var infoInDb = _context.Staffs
                .SingleOrDefault(t => t.StaffId == id);
            if (staffInDb == null || infoInDb == null)
            {
                return HttpNotFound();
            }
            _context.Users.Remove(staffInDb);
            _context.Staffs.Remove(infoInDb);
            _context.SaveChanges();
            return RedirectToAction("GetStaffs", "Admin");
        }

        public ActionResult ChangeStaffPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeStaffPassword(PasswordViewModel model, string id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userInDb = _context.Users
                .SingleOrDefault(i => i.Id == id);
            var result = await UserManager.ChangePasswordAsync(id, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(id);
                return RedirectToAction("GetStaffs", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        [HttpGet]
        public ActionResult GetTrainers(ManageMessageId? message)
        {
            ViewBag.StatusMessage = message == ManageMessageId.ChangePasswordSuccess ? "Password has been changed." : "";
            var trainers = _context.Trainers.ToList();
            return View(trainers);
        }

        [HttpGet]
        public ActionResult EditTrainer(int id)
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
        public ActionResult EditTrainer(Trainer trainer)
        {
            var trainerInDb = _context.Trainers.SingleOrDefault(t => t.Id == trainer.Id);
            if (trainerInDb == null)
            {
                return HttpNotFound();
            }
            trainerInDb.Name = trainer.Name;
            trainerInDb.Age = trainer.Age;
            trainerInDb.Address = trainer.Address;
            trainerInDb.Specialty = trainer.Specialty;

            _context.SaveChanges();
            return RedirectToAction("GetTrainers", "Admin");
        }

        [HttpGet]
        public ActionResult DeleteTrainer(string id)
        {
            var trainerInDb = _context.Users
                .SingleOrDefault(t => t.Id == id);
            var trainerInfoInDb = _context.Trainers
                .SingleOrDefault(t => t.TrainerId == id);
            if (trainerInfoInDb == null || trainerInfoInDb == null)
            {
                return HttpNotFound();
            }
            _context.Users.Remove(trainerInDb);
            _context.Trainers.Remove(trainerInfoInDb);
            _context.SaveChanges();
            return RedirectToAction("GetTrainers", "Admin");
        }

        public ActionResult ChangeTrainerPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeTrainerPassword(PasswordViewModel model, string id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userInDb = _context.Users
                .SingleOrDefault(i => i.Id == id);
            var result = await UserManager.ChangePasswordAsync(id, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(id);
                return RedirectToAction("GetTrainers", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
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