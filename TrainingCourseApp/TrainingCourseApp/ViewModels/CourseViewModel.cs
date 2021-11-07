using System.Collections.Generic;
using TrainingCourseApp.Models;

namespace TrainingCourseApp.ViewModels
{
    public class CourseViewModel
    {
        public Course Course { get; set; }
        public List<Category> Categories { get; set; }
    }
}