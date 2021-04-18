using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult AddNewCourse([FromQuery] string name, [FromQuery] string link)
        {
            var category = _context.Categories.Where(x => x.Name == name || x.Link == link).FirstOrDefault();
            if(category != null)
                return BadRequest(new {Message = "Category name of link is already exists"});

            category = new Category
            {
                Name = name,
                Link = link,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Categories.Add(category);
            _context.SaveChanges();
            return Ok(new {Message = "New Category Added"});
        }
        
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _context.Categories.ToList();
            return Ok(categories);
        }
    }
}