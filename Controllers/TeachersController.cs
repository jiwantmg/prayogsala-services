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

        [HttpPut]
        public ActionResult UpdateTeacher([FromBody]UserDto userDto) {
            // find the user
            var user = _context.Users.FirstOrDefault(x => x.UserId == userDto.UserId);
            if(user == null)
                return BadRequest(new { Message = "User not found" });
             
            user.Address = userDto.Address;
            user.Email = userDto.Email;                   
            user.FirstName = userDto.Fname;
            user.LastName = userDto.Lname;
            user.PhoneNo = userDto.Phone;                   

            _context.Users.Update(user);
            _context.SaveChanges();
            return Ok(new { Message = "User is updated"});
        }


        [HttpGet]
        public ActionResult GetTeachers()
        {
            var teachers = _context.Users.Where(x => x.Role == "teacher").ToList();
            return Ok(teachers);
        }
    }
}