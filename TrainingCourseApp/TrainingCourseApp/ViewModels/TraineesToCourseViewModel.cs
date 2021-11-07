using System.Collections.Generic;
using TrainingCourseApp.Models;

namespace TrainingCourseApp.ViewModels
{
    public class TraineesToCourseViewModel
    {
        public int CourseId { get; set; }
        public List<Course> Courses { get; set; }
        public int TraineeId { get; set; }
        public List<Trainee> Trainees { get; set; }
    }
}