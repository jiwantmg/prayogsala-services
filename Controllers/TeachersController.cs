using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PragyoSala.Services.Data;
using PragyoSala.Services.Dtos;
using PragyoSala.Services.Models;
using prayogsala_services.Util;

namespace PragyoSala.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeachersController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TeachersController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public ActionResult AddTeacher([FromBody]UserDto userDto) {
             try{
                var user = new User
                {
                    Address = userDto.Address,
                    Email = userDto.Email,
                    Password = UserUtil.ToSHA512(userDto.Password),
                    FirstName = userDto.Fname,
                    LastName = userDto.Lname,
                    PhoneNo = userDto.Phone,
                    Role = "teacher"
                };
                
                _context.Users.Add(user);
                _context.SaveChanges();
                return Ok(new
                {
                    Message = "New user created successfully"
                });
            }catch(Exception ex)
            {
                return BadRequest("Invalid request");
            }
        }

        [HttpGet]
        public ActionResult GetTeachers()
        {
            var teachers = _context.Users.Where(x => x.Role == "teacher").ToList();
            return Ok(teachers);
        }
    }
}