using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PragyoSala.Services.Data;
using PragyoSala.Services.Models;

namespace PragyoSala.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private AppDbContext _context;
        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult AddNew([FromBody] Category request)
        {
            var category = _context.Categories.Where(x => x.Name == request.Name || x.Link == request.Link).FirstOrDefault();
            if(category != null)
                return BadRequest(new {Message = "Category name or link is already exists"});

            category = new Category
            {
                Name = request.Name,
                Link = request.Link,
                DisplayHome = request.DisplayHome,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow                
            };

            _context.Categories.Add(category);
            _context.SaveChanges();
            return Ok(new {Message = "New Category Added"});
        }

        [HttpPut("{categoryId}")]
        public ActionResult UpdateCourse([FromRoute] int categoryId, [FromBody] Category category)
        {
            // check if category exists or not
            var cat = _context.Categories.FirstOrDefault(x => x.CategoryId == categoryId);
            if(cat == null)
                return NotFound(new {Message = "Category not found"});
            
            // update category
            cat.Name = category.Name;
            cat.Link = category.Link;
            cat.DisplayHome = category.DisplayHome;
            _context.Categories.Update(cat);            
            _context.SaveChanges();
            return Ok(new {Message = "Category updated"});
        }


        [HttpGet("{categoryLink}/courses")]
        public ActionResult GetCoursesByCategory([FromRoute] string categoryLink)
        {
            // get the category first
            var category = _context.Categories.FirstOrDefault(x => x.Link == categoryLink);
            if(category == null)
                return NotFound(new {Message = "Category with link does not found"});
            
            var courses = _context.Courses.Where(x => x.CategoryId == category.CategoryId).ToList();
            return Ok(courses);
        }
        
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _context.Categories.ToList();
            return Ok(categories);
        }

        [HttpGet("courses/tops")]
        public IActionResult GetCategoriesWithTop10()
        {
            var categories = _context.Categories
                                    .Where(x => x.DisplayHome)
                                    .Include(x => x.Courses.Take(10))                                    
                                        .ThenInclude( x => x.Rates.Where(r => r.Status))
                                    .ToList();
        
            return Ok(categories);

        }

    }
}