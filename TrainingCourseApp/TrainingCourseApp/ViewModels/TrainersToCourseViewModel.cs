using System.Collections.Generic;
using TrainingCourseApp.Models;

namespace TrainingCourseApp.ViewModels
{
    public class TrainersToCourseViewModel
    {
        public int CourseId { get; set; }
        public List<Course> Courses { get; set; }
        public int TrainerId { get; set; }
        public List<Trainer> Trainers { get; set; }
    }
}