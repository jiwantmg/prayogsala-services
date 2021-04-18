using System.IO;
using Microsoft.AspNetCore.Http;

namespace PragyoSala.Services.Dtos
{
    public class CourseDto
    {
        public string CourseTitle {get; set;}
        public int  CategoryId {get; set;}
        public IFormFile Thumbnail { get; set; }
    }
}