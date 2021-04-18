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
    [Route("api/public/courses")]
    public class PublicCoursesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthenticatedUserService _authenticatedUser;
        public PublicCoursesController(
            AppDbContext context,
            IAuthenticatedUserService authenticatedUser
            )
        {
            _context = context;
            _authenticatedUser = authenticatedUser;
        }

       
        [HttpGet]
        public ActionResult GetAllCourses()
        {
            var lists = _context.Courses.ToList();
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

     
    }
    
}