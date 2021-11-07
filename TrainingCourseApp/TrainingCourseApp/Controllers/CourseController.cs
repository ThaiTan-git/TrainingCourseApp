using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TrainingCourseApp.Models;
using TrainingCourseApp.Roles;
using TrainingCourseApp.ViewModels;

namespace TrainingCourseApp.Controllers
{
    [Authorize(Roles = Role.Staff)]
    public class CoursesController : Controller
    {
        // GET: Courses
        private ApplicationDbContext _context;
        public CoursesController()
        {
            _context = new ApplicationDbContext();
        }
        [HttpGet]
        public ActionResult Index(string SearchCourse)
        {
            var courses = _context.Courses
                .Include(t => t.Category)
                .ToList();

            if (!string.IsNullOrEmpty(SearchCourse))
            {
                courses = courses
                    .Where(t => t.Name
                    .ToLower()
                    .Contains(SearchCourse.ToLower()))
                    .ToList();
            }
            return View(courses);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var categories = _context.Categories.ToList();
            var viewModel = new CourseViewModel()
            {
                Categories = categories,
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Create(CourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CourseViewModel
                {
                    Course = model.Course,
                    Categories = _context.Categories.ToList()
                };
                return View(viewModel);
            }

            var newCourse = new Course()
            {
                Name = model.Course.Name,
                Description = model.Course.Description,
                CategoryId = model.Course.CategoryId,
            };
            _context.Courses.Add(newCourse);
            _context.SaveChanges();
            return RedirectToAction("Index", "Courses");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var courseInDb = _context.Courses
                .SingleOrDefault(t => t.Id == id);
            if (courseInDb == null)
            {
                return HttpNotFound();
            }
            var viewModel = new CourseViewModel
            {
                Course = courseInDb,
                Categories = _context.Categories.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Edit(CourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CourseViewModel
                {
                    Course = model.Course,
                    Categories = _context.Categories.ToList()
                };
                return View(viewModel);
            }
            var courseInDb = _context.Courses
                .SingleOrDefault(t => t.Id == model.Course.Id);
            if (courseInDb == null)
            {
                return HttpNotFound();
            }
            courseInDb.Name = model.Course.Name;
            courseInDb.Description = model.Course.Description;
            courseInDb.CategoryId = model.Course.CategoryId;
            _context.SaveChanges();
            return RedirectToAction("Index", "Courses");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var courseInDb = _context.Courses
                .SingleOrDefault(t => t.Id == id);
            if (courseInDb == null)
            {
                ModelState.AddModelError("", "Course is invalid");
                return RedirectToAction("Index", "Course");
            }
            _context.Courses.Remove(courseInDb);
            _context.SaveChanges();
            return RedirectToAction("Index", "Course");
        }

        [HttpGet]
        public ActionResult GetTrainees(string SearchCourse)
        {
            var courses = _context.Courses
                .Include(t => t.Category)
                .ToList();
            var trainee = _context.TraineeCourses.ToList();

            List<TraineesInCourseViewModel> viewModel = _context.TraineeCourses
                .GroupBy(i => i.Course)
                .Select(res => new TraineesInCourseViewModel
                {
                    Course = res.Key,
                    Trainees = res.Select(u => u.Trainee).ToList()
                })
                .ToList();
            if (!string.IsNullOrEmpty(SearchCourse))
            {
                viewModel = viewModel
                    .Where(t => t.Course.Name
                    .ToLower()
                    .Contains(SearchCourse.ToLower()))
                    .ToList();
            }
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult AddTrainee()
        {
            var viewModel = new TraineesToCourseViewModel
            {
                Courses = _context.Courses.ToList(),
                Trainees = _context.Trainees.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult AddTrainee(TraineesToCourseViewModel viewModel)
        {
            var model = new TraineeCourse
            {
                CourseId = viewModel.CourseId,
                TraineeId = viewModel.TraineeId
            };
            List<TraineeCourse> traineeCourses = _context.TraineeCourses.ToList();
            bool alreadyExist = traineeCourses.Any(item => item.CourseId == model.CourseId && item.TraineeId == model.TraineeId);
            if (alreadyExist == true)
            {
                ModelState.AddModelError("", "Trainee is already this Course");
                return RedirectToAction("GetTrainees", "Course");
            }
            _context.TraineeCourses.Add(model);
            _context.SaveChanges();

            return RedirectToAction("GetTrainees", "Course");
        }

        [HttpGet]
        public ActionResult RemoveTrainee()
        {
            var trainees = _context.TraineeCourses.Select(t => t.Trainee)
                .Distinct()
                .ToList();
            var courses = _context.TraineeCourses.Select(t => t.Course)
                .Distinct()
                .ToList();

            var viewModel = new TraineesToCourseViewModel
            {
                Courses = courses,
                Trainees = trainees
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult RemoveTrainee(TraineesToCourseViewModel viewModel)
        {
            var courseTrainee = _context.TraineeCourses
                .SingleOrDefault(t => t.CourseId == viewModel.CourseId && t.TraineeId == viewModel.TraineeId);
            if (courseTrainee == null)
            {
                ModelState.AddModelError("", "Trainee is not in this Course");
                return RedirectToAction("GetTrainees", "Course");
            }

            _context.TraineeCourses.Remove(courseTrainee);
            _context.SaveChanges();

            return RedirectToAction("GetTrainees", "Course");
        }

        [HttpGet]
        public ActionResult GetTrainers(string SearchCourse)
        {
            var courses = _context.Courses
                .Include(t => t.Category)
                .ToList();
            var trainer = _context.TrainerCourses.ToList();

            List<TrainersInCourseViewModel> viewModel = _context.TrainerCourses
                .GroupBy(i => i.Course)
                .Select(res => new TrainersInCourseViewModel
                {
                    Course = res.Key,
                    Trainers = res.Select(u => u.Trainer).ToList()
                })
                .ToList();
            if (!string.IsNullOrEmpty(SearchCourse))
            {
                viewModel = viewModel
                    .Where(t => t.Course.Name.ToLower().Contains(SearchCourse.ToLower())).
                    ToList();
            }
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult AddTrainer()
        {
            var viewModel = new TrainersToCourseViewModel
            {
                Courses = _context.Courses.ToList(),
                Trainers = _context.Trainers.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult AddTrainer(TrainersToCourseViewModel viewModel)
        {
            var model = new TrainerCourse
            {
                CourseId = viewModel.CourseId,
                TrainerId = viewModel.TrainerId
            };

            List<TrainerCourse> trainerCourses = _context.TrainerCourses.ToList();
            bool alreadyExist = trainerCourses.Any(item => item.CourseId == model.CourseId && item.TrainerId == model.TrainerId);
            if (alreadyExist == true)
            {
                ModelState.AddModelError("", "Trainer is already this Course");
                return RedirectToAction("GetTrainers", "Course");
            }
            _context.TrainerCourses.Add(model);
            _context.SaveChanges();

            return RedirectToAction("GetTrainers", "Course");
        }

        [HttpGet]
        public ActionResult RemoveTrainer()
        {
            var trainers = _context.TrainerCourses.Select(t => t.Trainer)
                .Distinct()
                .ToList();
            var courses = _context.TrainerCourses.Select(t => t.Course)
                .Distinct()
                .ToList();

            var viewModel = new TrainersToCourseViewModel
            {
                Courses = courses,
                Trainers = trainers
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult RemoveTrainer(TrainersToCourseViewModel viewModel)
        {
            var courseTrainer = _context.TrainerCourses
                .SingleOrDefault(t => t.CourseId == viewModel.CourseId && t.TrainerId == viewModel.TrainerId);
            if (courseTrainer == null)
            {
                ModelState.AddModelError("", "Trainer is not in this Course");
                return RedirectToAction("GetTrainers", "Course");
            }

            _context.TrainerCourses.Remove(courseTrainer);
            _context.SaveChanges();

            return RedirectToAction("GetTrainers", "Course");
        }

    }
}