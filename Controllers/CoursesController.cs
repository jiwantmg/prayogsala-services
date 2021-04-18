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
            course.CategoryId = courseDto.CategoryId;  
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
                        .ThenInclude(x => x.Video)
                .FirstOrDefault(x => x.CourseId == courseId);
            return Ok(course);
        }

        [HttpGet("tops")]
        public ActionResult GetTopCourses()
        {
            var lists = _context.Courses.Where(x => 
                x.UserId == _authenticatedUser.UserId
            ).Select(c => new {
                  CourseId = c.CourseId,
                  CourseTitle = c.CourseTitle,      
                  Image = c.Image,
                  StudentsCount = c.CourseStudents.Count()
            })
            .OrderByDescending(x => x.StudentsCount)
            .Take(10)
            .ToList();

            return Ok(lists);
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

        [HttpPost("{courseId}/chapters/{chapterId}/topics/{topicId}/video")]
        public ActionResult AddNewVideo(
            [FromRoute] int courseId,
            [FromRoute] int chapterId,
            [FromRoute] int topicId,
            [FromForm] VideoUploadDto videoDto
            )
        {
            var course = _context.Courses.Where(x => x.CourseId == courseId).FirstOrDefault();
            if(course == null)
            {
                return BadRequest(new {Message = "Course not found"});
            }

            var chapter = _context.Chapters.Where(x => x.ChapterId == chapterId && x.CourseId == courseId).FirstOrDefault();
            if(chapter == null)
            {
                return BadRequest(new {Message = "Chapter not found"});
            }
            var topic = _context.Topics.Where(x => x.ChapterId == chapterId && x.CourseId == courseId && x.TopicId == topicId).FirstOrDefault();
            if(topic == null)
            {
                return BadRequest(new {Message = "Topic not found"});
            }

            string videoName = Guid.NewGuid().ToString() + Path.GetExtension(videoDto.Video.FileName);
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads\\Videos", videoName);            
            using(var stream = new FileStream(savePath, FileMode.CreateNew))
            {
                videoDto.Video.CopyTo(stream);
            }

            var video = new Video
            {
                VideoName = videoName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow            
            };

            topic.Video = video;
            _context.Topics.Update(topic);
            _context.SaveChanges();

            // course = new Course();
            // course.CourseTitle = courseDto.CourseTitle;
            // course.Image = imageName;
            // course.CreatedAt = DateTime.UtcNow;
            // course.UpdatedAt = DateTime.UtcNow;
            // course.UserId = _authenticatedUser.UserId;
            // _context.Courses.Add(course);
            // _context.SaveChanges();
            
            return Ok(new {Message = "New course added"});
        }
    }
    
}