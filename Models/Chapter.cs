using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PragyoSala.Services.Models
{
    [Table("chapters")]
    public class Chapter
    {
        public int ChapterId { get; set; }
        public int Order { get; set; }          
        public string ChapterName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }

        // navigational property
        public int CourseId { get; set; }
        public Course Course { get; set; }    
        public List<Topic> Topics { get; set; }
    }
}