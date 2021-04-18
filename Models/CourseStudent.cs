using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PragyoSala.Services.Models
{
    [Table("course_students")]
    public class CourseStudent
    {
        public int CourseStudentId { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}