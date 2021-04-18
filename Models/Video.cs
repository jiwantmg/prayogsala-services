using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PragyoSala.Services.Models
{
    [Table("videos")]
    public class Video
    {
        public int VideoId { get; set; }
        public string VideoName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}