using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PragyoSala.Services.Data;
using PragyoSala.Services.Dtos;
using System;
using System.Linq;
using System.IO;
using PragyoSala.Services.Models;
using prayogsala_services.Middlewares;

namespace PragyoSala.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthenticatedUserService _authenticatedUser;
        public CoursesController(
            AppDbContext context,
            IAuthenticatedUserService authenticatedUser
            )
        {
            _context = context;
            _authenticatedUser = authenticatedUser;
        }

        [HttpPost]
        public ActionResult AddNewCourse([FromForm] CourseDto courseDto)
        {
            var course = _context.Courses.Where(x => x.CourseTitle == courseDto.CourseTitle).FirstOrDefault();
            if(course != null)
            {
                return BadRequest(new {Message = "Course name already exists"});
            }

            if(courseDto.Thumbnail == null)
                return BadRequest(new {Message = "Thumbnail cannot be empty"});

            string imageName = Guid.NewGuid().ToString() + Path.GetExtension(courseDto.Thumbnail.FileName);
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", imageName);            
            using(var stream = new FileStream(savePath, FileMode.CreateNew))
            {
                courseDto.Thumbnail.CopyTo(stream);
            }

            course = new Course();
            course.CourseTitle = courseDto.CourseTitle;
            course.Image = imageName;
            course.CreatedAt = DateTime.UtcNow;
            course.UpdatedAt = DateTime.UtcNow;
            course.UserId = _authenticatedUser.UserId;
            _context.Courses.Add(course);
            _context.SaveChanges();
            
            return Ok(new {Message = "New course added"});
        }
        [HttpGet]
        public ActionResult GetAllCourses()
        {
            var lists = _context.Courses.Where(x => x.UserId == _authenticatedUser.UserId);
            return Ok(lists);
        }

        [HttpGet("{courseId}")]
        public ActionResult GetCourse([FromRoute] int courseId)
        {
            var course = _context.Courses
                .Include(x => x.Chapters)
                    .ThenInclude(x => x.Topics)
                .FirstOrDefault(x => x.CourseId == courseId);
            return Ok(course);
        }

        [HttpPost("{courseId}/chapters")]
        public ActionResult AddNewChapter([FromBody] ChpaterDto chapterDto)
        {
            var course = _context.Courses.FirstOrDefault(x => x.CourseId == chapterDto.CourseId);

            if(course == null)
                return BadRequest(new { Message = "Course is not found"});
            
            var chapter = new Chapter
            {
                ChapterName = chapterDto.ChapterName,
                Order = chapterDto.Order,
                CourseId = chapterDto.CourseId
            };

            _context.Chapters.Add(chapter);
            _context.SaveChanges();

            return Ok(new {Message = "New Chapter created"});

        }

        [HttpPost("{courseId}/chapters/{chapterId}/topics")]
        public IActionResult AddNewTopic([FromRoute] int courseId, [FromRoute] int chapterId, [FromBody] TopicDto topicDto)
        {
            var course = _context.Courses.FirstOrDefault(x => x.CourseId == courseId);
            if(course == null)
                return BadRequest(new { Message = "Course not found" });

            var chapter = _context.Chapters.FirstOrDefault(x => x.ChapterId == chapterId && x.CourseId == courseId);
            if(chapter == null)
                return BadRequest(new {Message = "Chapter not found"});

            var topic = new Topic
            {
                ChapterId = chapterId,
                CourseId = courseId,
                Order = topicDto.Order,
                TopicName = topicDto.Topic                
            };

            _context.Topics.Add(topic);
            _context.SaveChanges();

            return Ok(new {Message = "new topic created"});

        }
    }
}