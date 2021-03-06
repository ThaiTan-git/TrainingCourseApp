using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace TrainingCourseApp.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("TrainingCourseApp", throwIfV1Schema: false)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Staff> Staffs { get; set; }

        public DbSet<Trainer> Trainers { get; set; }

        public DbSet<Trainee> Trainees { get; set; }

        public DbSet<Course> Courses { get; set; }
        public DbSet<TraineeCourse> TraineeCourses { get; set; }

        public DbSet<TrainerCourse> TrainerCourses { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}