using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PragyoSala.Services.Models
{
    [Table("courses")]
    public class Course
    {
        public int CourseId { get; set; }        
        public string CourseTitle { get; set; }        
        public string Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set;}        


        // navigational properties
        public int UserId { get; set; }
        public User User { get; set; }
        public List<CourseRate> Rates { get; set; }
        public List<Chapter> Chapters { get; set; }
        public List<CourseStudent> CourseStudents { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set;}
    }
}