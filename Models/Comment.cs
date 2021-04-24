using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PragyoSala.Services.Models
{
    [Table("comments")]
    public class Comment
    {
        public int CommentId { get; set; }
        public string Text { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int ChapterId { get; set; }
        public Chapter Chapter { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
        public int UserId { get; set; }        
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}
