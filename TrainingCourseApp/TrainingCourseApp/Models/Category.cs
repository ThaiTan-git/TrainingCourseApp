﻿using System.ComponentModel.DataAnnotations;

namespace TrainingCourseApp.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}