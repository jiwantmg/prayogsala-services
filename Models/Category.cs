using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PragyoSala.Services.Models
{
    [Table("categories")]
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        [DefaultValue(false)]
        public bool DisplayHome { get; set; }
        public List<Course> Courses{ get; set; }
        public DateTime CreatedAt { get; set; }        
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}