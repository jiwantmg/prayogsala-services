using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PragyoSala.Services.Data;
using PragyoSala.Services.Dtos;
using System;
using System.Linq;
using System.IO;
using PragyoSala.Services.Models;
using prayogsala_services.Middlewares;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PragyoSala.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthenticatedUserService _authenticatedUser;
        private string baseurl = "https://khalti.com/api/v2/payment/verify/";
        private string key = "test_secret_key_e6c15ee8d5af48c9a4d0dd66662aaca7";
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
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"+Path.DirectorySeparatorChar+"Uploads", imageName);            
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
        [HttpGet("{courseType}/all")]
        public ActionResult GetAllCourses([FromRoute] string courseType)
        {
            List<Course> lists = new List<Course>();
            if(courseType == "teacher" && _authenticatedUser.Role == "teacher") {
                lists = _context.Courses.Where(x => x.UserId == _authenticatedUser.UserId).ToList();
            }                
            else if(courseType == "admin" && _authenticatedUser.Role == "admin") {
                lists = _context.Courses.Where(x => x.UserId == _authenticatedUser.UserId).ToList();
            }                
            else {
                // find courses enrolled by the student
                var enrolledCourses = _context.CourseStudents
                                                .Where(x => x.UserId == _authenticatedUser.UserId)
                                                .Include(x => x.Course)                                            
                                                .ToList();
                foreach(var eC in enrolledCourses)
                {
                    lists.Add(eC.Course);
                }
            }
            return Ok(lists);
        }

        [HttpGet("{courseId}")]
        public ActionResult GetCourse([FromRoute] int courseId)
        {
            var course = _context.Courses
                .Include(x => x.Chapters)
                    .ThenInclude(x => x.Topics)
                        .ThenInclude(x => x.Video)
                .Include(x => x.Rates.Where(x => x.Status).Take(1))
                .FirstOrDefault(x => x.CourseId == courseId);
            // check if this course is paid by the user or not
            var paidStatus = _context.CourseStudents.FirstOrDefault(x => x.CourseId == courseId && x.UserId == _authenticatedUser.UserId);            
            return Ok(new {
                course,
                paidStatus
            });
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
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"+Path.DirectorySeparatorChar+"Uploads"+Path.DirectorySeparatorChar+"Videos", videoName);            
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

        [HttpPut("enroll/{courseId}")]
        public IActionResult EnrollStudent([FromRoute] int courseId)
        {
            // check if course exists or not
            var course = _context.Courses.FirstOrDefault(x => x.CourseId == courseId);
            if(course == null)
                return BadRequest(new { Message = "Course not found" });
            
            // check if student is already enrolled 
            var studentCourse = _context.CourseStudents.FirstOrDefault(x => x.CourseId == courseId && x.UserId == _authenticatedUser.UserId);
            if(studentCourse != null)
                return BadRequest(new { Message = "Student is already enroled"});

            studentCourse = new CourseStudent()
            {
                CourseId = course.CourseId,
                UserId = _authenticatedUser.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = "enrolled"                
            };

            _context.Add(studentCourse);
            _context.SaveChanges();

            return Ok(new {Message = "New student saved"});
        }
 
        [HttpPost("payment/verify/{courseId}")]
        public async Task<ActionResult> VerifyPaymentForOrder([FromRoute] int courseId, [FromBody] VerifyPayment command, CancellationToken cancellationToken)
        {
            var data = new {token = command.Token, amount = command.Amount };            
            
            using(HttpClient httpClient = new HttpClient())
            {                
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Key", key);
                var payload = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                using(HttpResponseMessage res = await httpClient.PostAsync(baseurl, payload))
                {
                    if(res.IsSuccessStatusCode)
                    {
                        using(HttpContent content = res.Content)
                        {
                            string response = await content.ReadAsStringAsync();
                            Console.WriteLine(response);
                            // update course as paid
                            var studentCourse = _context.CourseStudents.FirstOrDefault(
                                x => x.CourseId == courseId
                                && x.UserId == _authenticatedUser.UserId
                            );

                            if(studentCourse == null)
                                return BadRequest(new { Message = "Course not found" });
                            
                            studentCourse.Status = "purchased";                            
                            _context.CourseStudents.Update(studentCourse);
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        return BadRequest(new { Message = "Something went wrong while making a payment, please try again" });
                    }             
                }
            }
            return Ok(new {Message = "Payment made successfull"});
       }

       [HttpGet("search")]
       public IActionResult Search([FromQuery] string query)
       {
           var courses = _context.Courses.Where(x => x.CourseTitle.Contains(query)).ToList();
           return Ok(courses);
       }

       [HttpGet("{courseId}/rates")]
       public IActionResult GetCourseRates([FromRoute] int courseId)
       {
           // find if course Id exists
           var course = _context.Courses.FirstOrDefault(x => x.CourseId == courseId);
           if(course == null)
            return BadRequest(new {Message = "Course does not found"});

           // find all rates of the course
           var rates = _context.CourseRates.Where(x => x.CourseId == courseId).OrderByDescending(x => x.CratedAt).ToList();
           return Ok(rates);
       }

       [HttpPost("{courseId}/rates")]
       public IActionResult AddNewRates([FromBody] NewRate rate, [FromRoute] int courseId)
       {
            if(rate.Rate <= 0)
                return BadRequest(new {Message = "Please provide course rate > 0"});           

            // update the rate also
            var oldRate = _context.CourseRates.FirstOrDefault(x => x.Status && x.CourseId == courseId);
            if(oldRate != null)
            {
                oldRate.Status = false;
                _context.CourseRates.Update(oldRate);
            }            

            var courseRate = new CourseRate
            {
                CourseId = courseId,
                Rate = rate.Rate,
                CratedAt = DateTime.Now,
                Status = true
            };

            _context.CourseRates.Add(courseRate);
            _context.SaveChanges();
            return Ok(new {Message = "New Rate added to course"});
       }
    }
}