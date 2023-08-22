using Captureit.Data;
using Captureit.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Captureit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;

        public UserController(ApplicationDbContext Context)
        {
            _Context = Context;
        }
        //For Getting Details of all students 
        [HttpGet]
        [Route("GetStudent")]
        public async Task<IActionResult> GetAllUsers()
        {
            var students = await _Context.Users.Where(u => u.Role == "Student").ToListAsync();
            return Ok(students);
        }
        [HttpGet]
        [Route("GetTeacher")]
        public async Task<IActionResult> GetAllTeacher()
        {
            var teachers = await _Context.Users.Where(u => u.Role == "Teacher").ToListAsync();
            return Ok(teachers);
        }
        //For Getting the data of single Student
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetSingleUser(int id)
        {
            var s = await _Context.Users.FindAsync(id);
            if (s == null)
            {
                return NotFound();
            }
            return Ok(s);

        }
        //For Storing the data in Database
        [HttpPost]
        [Route("addstudent")]
        public async Task<IActionResult> AddAllUser(User u)
        {
            var isalreadyexist = await CheckDuplicateRollNo(u.RollNo);
            if (isalreadyexist)
            {
                return Ok(null);
                //return BadRequest("User already Exist");
            }

            var s = new User()
            {             
                RollNo = u.RollNo,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                ContactNo = u.ContactNo,
                Password = "123456789",
                Department = u.Department,
                ReferenceMobileNo = u.ReferenceMobileNo,
                ReferenceEmail = u.ReferenceEmail,
                Semester = u.Semester,
                Role = "Student",
            };
            await _Context.Users.AddAsync(s);
            await _Context.SaveChangesAsync();
            return Ok(s);
        }
        private async Task<bool> CheckDuplicateRollNo(string rollNo)
        {
            var existingUser = await _Context.Users.FirstOrDefaultAsync(user => user.RollNo.ToLower() == rollNo.ToLower());
            if (existingUser != null)
            {
                return true;
            }
            return false;
        }
        [HttpPost]
        [Route("addteacher")]
      
        public async Task<IActionResult> AddAllUsers(User u)
        {
            
            var s = new User()
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                ContactNo = u.ContactNo,
               
                Password = "123456789",
                Department = u.Department,
                //Department = u.Department,
                // ReferenceMobileNo = u.ReferenceMobileNo,
                //ReferenceEmail = u.ReferenceEmail,
                //Semester = u.Semester,
                Role = "Teacher",
            };
            await _Context.Users.AddAsync(s);
            await _Context.SaveChangesAsync();
            return Ok(s);
        }
        [HttpGet]
        [Route("getbtech")]
        public async Task<IActionResult> GetDep1()
        {
            
            var btech = await _Context.Users.Where(u => u.Role == "Student" && u.Department == "B-Tech").ToListAsync();
            if(btech != null)
            {
                return Ok(btech);

            }
            return Ok("No Data found");
            
        }
        [HttpGet]
        [Route("getmtech")]
        public async Task<IActionResult> GetDep2()
        {
            var mtech = await _Context.Users.Where(u => u.Role == "Student" && u.Department == "M-Tech").ToListAsync();
            return Ok(mtech);
        }
        [HttpGet]
        [Route("getbca")]
        public async Task<IActionResult> GetDep3()
        {
            var bca = await _Context.Users.Where(u => u.Role == "Student" && u.Department == "BCA").ToListAsync();
            return Ok(bca);
        }
        [HttpGet]
        [Route("getmca")]
        public async Task<IActionResult> GetDep4()
        {
            var mca = await _Context.Users.Where(u => u.Role == "Student" && u.Department == "MCA").ToListAsync();
            return Ok(mca);

        }


        [HttpDelete]
        [Route("deletestudent")]
        public bool DeleteStudent(string rollNo)
        {
                User u = _Context.Users.FirstOrDefault(each => each.RollNo == rollNo);
                if (u != null)
                {
                    _Context.Remove(u);
                    _Context.SaveChanges();
                    return true;
                }
                return false;            
        }
        [HttpDelete]
        [Route("deleteteacher")]
        public bool DeleteTeacher(int id)
        {
            User u = _Context.Users.FirstOrDefault(each => each.Id == id && each.Role == "Teacher");
            if (u != null)
            {
                _Context.Remove(u);
                _Context.SaveChanges();
                return true;
            }
            return false;
        }
        [HttpGet]
        [Route("loginStudent")]
        public async Task<IActionResult> LoginStudent(string Email)
        {
            var students = await _Context.Users.Where(u => u.Email == Email && u.Password == "123456789" &&  u.Role == "Student").ToListAsync();
            return Ok(students);
        }
        [HttpGet]
        [Route("loginTeacher")]
        public async Task<IActionResult> LoginTeacher(string Email)
        {
            var teacher = await _Context.Users.Where(u => u.Email == Email && u.Password == "123456789" && u.Role == "Teacher").ToListAsync();
            return Ok(teacher);
        }
        [HttpGet]
        [Route("getattendancestudent")]
        public async Task<IActionResult> getattendance(string Email)
        {
            var students = await _Context.Users.Where(u => u.Email == Email && u.Role == "Student").ToListAsync();
            return Ok(students);
        }

        [HttpPost]
        [Route("takeimage")]
        public async Task<IActionResult> takeimage(string email, [FromBody] object u)
        {




            // Convert the image data from base64 string to byte array
            // byte[] i = Convert.FromBase64String(u.Image);
            // string img = Convert.ToBase64String(i);



            var student = await _Context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (student == null)
            {
                return NotFound();
            }
            else
            {
                student.Image = u.ToString();
                await _Context.SaveChangesAsync();

            }



            // Create a new instance of the image entity
            //User image = new User()
            //{
            //    // Set the image data to the byte array
            //    Image = img,
            //    //Id = u.Id,
            //};



            //_Context.Users.Add(image);

            // Return the success response
            return Ok();

        }
        [HttpGet]
        [Route("teacherAttendance")]
        public async Task<IActionResult> TeacherAttendance(string Email)
        {
            var teacher = await _Context.Users.FirstOrDefaultAsync(u => u.Email == Email && u.Password == "123456789" && u.Role == "Teacher");





            if (teacher == null)
            {
                return Ok(null);
                //return BadRequest("Invalid teacher credentials");
            }





            var students = await _Context.Users.Where(u => u.Department == teacher.Department && u.Role == "Student").ToListAsync();
            var attendance = new List<object>();





            foreach (var student in students)
            {
                var studentAttendance = await _Context.Users.FirstOrDefaultAsync(a => a.Id == student.Id);
                if (studentAttendance != null)
                {
                    attendance.Add(new { studentRollNo = student.RollNo, StudentName = student.FirstName, studentLname = student.LastName, studentClass = student.Semester, Attendance = studentAttendance.Attendance, studentEmail = student.Email });
                }
            }



            return Ok(attendance);
        }


    }
}

