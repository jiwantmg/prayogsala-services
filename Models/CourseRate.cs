using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PragyoSala.Services.Models
{
    [Table("course_rate")]
    public class CourseRate
    {
        public int CourseRateId { get; set; }
        public double Rate { get; set; }   
        public int CourseId { get; set; }
        public bool Status { get; set; }
        public Course Course { get; set;}
        public DateTime CratedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}