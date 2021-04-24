using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PragyoSala.Services.Data;
using PragyoSala.Services.Dtos;
using PragyoSala.Services.Models;
using prayogsala_services.Middlewares;

namespace PragyoSala.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly IAuthenticatedUserService _authenticatedUser;
        private readonly AppDbContext _context;
        public CommentsController(
            IAuthenticatedUserService authenticatedUser,
            AppDbContext context
        )
        {
            _authenticatedUser = authenticatedUser;
            _context = context;
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddNewComment([FromBody] CommentRequest commentRequest)
        {
            var comment = new Comment()
            {
                Text = commentRequest.Text,
                UserId = _authenticatedUser.UserId,
                CourseId = commentRequest.CourseId,
                ChapterId = commentRequest.ChapterId,
                TopicId = commentRequest.TopicId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now                
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return Ok(new { Message = "New comment saved"});
        }

        [HttpGet("topics/{topicId}")]
        public IActionResult GetCommentsByTopic([FromRoute] int topicId)
        {
            var result = _context.Comments
                    .Where(x => x.TopicId == topicId)
                    .Include(x => x.User)
                    .ToList();
            return Ok(result);
        }
    }
}