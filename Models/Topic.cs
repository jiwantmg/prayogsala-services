using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PragyoSala.Services.Models
{
    [Table("topics")]
    public class Topic
    {
        public int TopicId { get; set; }
        public int Order { get; set; }
        public string TopicName { get; set; }       
        public double Length { get; set; }
        public bool IsWatched { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        // navigational property
        public int  ChapterId {get; set; }
        public Chapter Chapter { get; set;}
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int? VideoId { get; set; }
        public Video Video { get; set;}
    }
}