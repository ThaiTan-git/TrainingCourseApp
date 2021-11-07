using System.Collections.Generic;
using TrainingCourseApp.Models;

namespace TrainingCourseApp.ViewModels
{
    public class TrainersInCourseViewModel
    {
        public Course Course { get; set; }
        public List<Trainer> Trainers { get; set; }
    }
}