using System.Collections.Generic;
using TrainingCourseApp.Models;

namespace TrainingCourseApp.ViewModels
{
    public class TraineesInCourseViewModel
    {
        public Course Course { get; set; }
        public List<Trainee> Trainees { get; set; }
    }
}