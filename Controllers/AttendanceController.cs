////using Microsoft.AspNetCore.Mvc;
////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Net.Http;
////using System.Threading.Tasks;
////using Captureit.DataFactory.Models;
////using static System.Console;
////using System.Net.Mail;
////using Captureit.Data;
////using Microsoft.EntityFrameworkCore;

////namespace Captureit.Controllers
////{
////    public class AttendanceController : ControllerBase

////    {
////        private readonly ApplicationDbContext _context;
////        public AttendanceController(DbContextOptions<ApplicationDbContext> options)
////        {
////            _context = new ApplicationDbContext(options);
////        }


////        #region Methods
////        [HttpGet]
////        [Route("captureAttendance")]
////        public async Task<IActionResult> captureAttendance(attendanceInfo data)
////        {
////            try
////            {
////                HttpClient client = new HttpClient();
////                client.BaseAddress = new Uri("http://192.168.116.22:5003/");
////                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
////                HttpResponseMessage response = client.GetAsync("Camera/camera/capture").Result;

////                if (response.IsSuccessStatusCode)
////                {
////                    byte[] imageBytes = response.Content.ReadAsByteArrayAsync().Result;
////                    // Convert this into base64
////                    data.attnImage = Convert.ToBase64String(imageBytes);
////                    data.date = DateTime.Now;
////                    // string store in db

////                    // async function 
////                    // user table 6 students present... default 0, each time +20
////                    await insertAttn(data);
////                    return File(imageBytes, "image/*");
////                }
////                else
////                {
////                    return BadRequest("Something went wrong");
////                }
////            }
////            catch (Exception e)
////            {
////                return Ok(e.Message);
////            }
////        }

////        #endregion

////        #region Private Methods
////        private async Task insertAttn(attendanceInfo data)
////        {
////            try
////            {
////                // Read from database of student User's Fname, Lname, rollno, dept, mailid, pmail attn based on dept 
////                using (_context)
////                {
////                    var users = await _context.Users

////                        .Where(u => u.Department == data.dept)
////                        .Select(u => new
////                        {
////                            u.FirstName,
////                            u.LastName,
////                            u.RollNo,
////                            u.Department,
////                            u.Email,
////                            u.ReferenceEmail,
////                            u.Attendance
////                        })
////                        .ToListAsync();

////                    // Existing attn + 20 add
////                    foreach (var user in users)
////                    {
////                        //user.Attendance += 20;
////                        //context.Entry(user).State = EntityState.Modified;
////                    }

////                    await _context.SaveChangesAsync();

////                    // Create absentAlert for absent users and call sendAbsenceMailNotification
////                    var absentUsers = users.Where(u => u.Attendance < 60); // Users with less than 60% attendance are considered absent
////                    var absentStudentRecords = new List<absenceMailNotification>();
////                    foreach (var user in absentUsers)
////                    {
////                        var absentStudentRecord = new absenceMailNotification()
////                        {
////                            FirstName = user.FirstName,
////                            LastName = user.LastName,
////                            RollNo = user.RollNo,
////                            Department = user.Department,
////                           StudentMail = user.Email,
////                            ParentMail = user.ReferenceEmail
////                        };
////                        absentStudentRecords.Add(absentStudentRecord);
////                    }

////                    //if (absentStudentRecords.Count > 0)
////                    //{
////                    //    var absentAlert = new
////                    //    {
////                    //        Department = data.dept,
////                    //        Absentees = absentStudentRecords
////                    //    };

////                    //    await sendAbsenceMailNotification(absentAlert);
////                    //}
////                    if (absentStudentRecords.Count > 0)
////                    {
////                        var absentAlert = new absenceAlert
////                        {
////                            Department = data.dept,
////                            Absentees = absentStudentRecords
////                        };

////                        await sendAbsenceMailNotification(absentAlert);
////                    }

////                }
////            }
////            catch (Exception ex)
////            {
////                WriteLine("Exception while inserting attn");
////                WriteLine(ex);
////            }

////        }

////        private async Task sendAbsenceMailNotification(absenceAlert records)
////        {
////            try
////            {
////                if (records.Absentees.Count > 0)
////                {
////                    for (int count = 0; count < records.Absentees.Count; count++)
////                    {
////                        await SendMail(records.Absentees[count]);
////                        Task.Delay(1000);
////                    }
////                }
////            }
////            catch (Exception ex)
////            {
////                WriteLine("Exception while sending absent mail notifications.");
////                WriteLine(ex);
////            }
////        }


////        private async Task SendMail(absenceMailNotification student)
////        {
////            try
////            {
////                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
////                smtpClient.UseDefaultCredentials = false; // uncomment if you don't want to use the network credentials
////                smtpClient.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
////                smtpClient.Credentials = new System.Net.NetworkCredential("ayushmodi2612@gmail.com", "sysmyylgnjeexhjn");
////                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
////                smtpClient.EnableSsl = true;
////                MailMessage mail = new MailMessage();
////                mail.From = new MailAddress("ayushmodi2612@gmail.com", "CHARUSAT ADMIN");
////                mail.To.Add(new MailAddress(student.ParentMail));
////                mail.CC.Add(new MailAddress(student.StudentMail));
////                mail.Subject = "Attendance Notification";
////                mail.Body = student.FirstName + " " + student.LastName + " (" + student.RollNo + ") from dept " + student.Department + " is absent today.";
////                smtpClient.Send(mail);
////            }
////            catch (Exception ex)
////            {
////                WriteLine("Exception while sending mail");
////                WriteLine(ex);
////            }
////        }
////        #endregion
////    }
////}
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Captureit.DataFactory.Models;
//using static System.Console;
//using System.Net.Mail;
//using Captureit.Data;
//using Microsoft.EntityFrameworkCore;

//namespace Captureit.Controllers
//{
//    public class AttendanceController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;
//        public AttendanceController(DbContextOptions<ApplicationDbContext> options)
//        {
//            _context = new ApplicationDbContext(options);
//        }

//        #region Methods
//        [HttpPost]
//        [Route("captureAttendance")]
//        public async Task<IActionResult> captureAttendance(attendanceInfo data)
//        {
//            try
//            {
//                HttpClient client = new HttpClient();
//                client.BaseAddress = new Uri("http://10.126.67.172:5003/");
//                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
//                HttpResponseMessage response = client.GetAsync("Camera/camera/capture").Result;

//                if (response.IsSuccessStatusCode)
//                {
//                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
//                    // Convert this into base64
//                    data.attnImage = Convert.ToBase64String(imageBytes);
//                    data.date = DateTime.Now;
//                    // string store in db

//                    // user table 6 students present... default 0, each time +20
//                    await insertAttn(data);
//                    return File(imageBytes, "image/*");
//                }
//                else
//                {
//                    return BadRequest("Something went wrong");
//                }
//            }
//            catch (Exception e)
//            {
//                return Ok(e.Message);
//            }
//        }

//        #endregion

//        #region Private Methods


//        private async Task insertAttn(attendanceInfo data)
//        {
//            try
//            {
//                // Read from database of student User's Fname, Lname, rollno, dept, mailid, pmail attn based on dept 
//                using (_context)
//                {
//                    var users = await _context.Users
//                        .Where(u => u.Department == data.dept)
//                        .Select(u => new
//                        {
//                            u.FirstName,
//                            u.LastName,
//                            u.RollNo,
//                            u.Department,
//                            u.Email,
//                            u.ReferenceEmail,
//                            u.Attendance
//                        })
//                        .ToListAsync();

//                    // Existing attn + 20 add
//                    foreach (var user in users)
//                    {
//                        //user.Attendance += 20;
//                        //context.Entry(user).State = EntityState.Modified;
//                    }

//                    await _context.SaveChangesAsync();

//                    // Create absentAlert for absent users and call sendAbsenceMailNotification
//                    var presentStudents = users.Select(u => u.RollNo).ToList();
//                    var allStudents = await _context.Users
//                        .Where(u => u.Department == data.dept)
//                        .Select(u => new
//                        {
//                            u.FirstName,
//                            u.LastName,
//                            u.RollNo,
//                            u.Department,
//                            u.Email,
//                            u.ReferenceEmail,
//                            u.Attendance
//                        })
//                        .ToListAsync();
//                    var absentStudents = allStudents.Where(s => !presentStudents.Contains(s.RollNo)).ToList();
//                    var absentStudentRecords = absentStudents.Select(s => new absenceMailNotification
//                    {
//                        FirstName = s.FirstName,
//                        LastName = s.LastName,
//                        StudentMail = s.Email,
//                        Department = s.Department
//                    }).ToList();

//                    // Send email to all recipients
//                    var emailTask = sendAbsenceMailNotification(absentStudentRecords);
//                    // Warning: because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
//                }
//                await _context.SaveChangesAsync();
//            }

//            catch (Exception e)
//            {
//                Console.WriteLine(e.Message);
//            }
//        }

//        private async Task sendAbsenceMailNotification(List<absenceMailNotification> absentees)
//        {
//            try
//            {
//                // Mail setup
//                var smtpClient = new SmtpClient("smtp.gmail.com")
//                {
//                    Port = 587,
//                    Credentials = new System.Net.NetworkCredential("myemail@gmail.com", "mypassword"),
//                    EnableSsl = true,
//                };
//                var mailMessage = new MailMessage();
//                mailMessage.From = new MailAddress("myemail@gmail.com");
//                foreach (var absentee in absentees)
//                {
//                    mailMessage.To.Add(new MailAddress(absentee.StudentMail));
//                }
//                mailMessage.Subject = "Regarding Absent in Class";
//                mailMessage.Body = "You are marked absent for today's class.";
//                await smtpClient.SendMailAsync(mailMessage);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.Message);
//            }
//        }

//        #endregion
//    }
//}





using Microsoft.AspNetCore.Mvc;

using System;

using System.Collections.Generic;

using System.Linq;

using System.Net.Http;

using System.Threading.Tasks;

using Captureit.DataFactory.Models;

using static System.Console;

using System.Net.Mail;

using Captureit.Data;

using Microsoft.EntityFrameworkCore;
using Captureit.Services;

namespace Captureit.Controllers

{
    public class AttendanceController : ControllerBase
    {
        private ApplicationDbContext _context;
//private static MailTimer _mailTimer;
        public AttendanceController(DbContextOptions<ApplicationDbContext> options)
        {

            _context = new ApplicationDbContext(options);

        }
        #region Methods

        [HttpPost]

        [Route("captureAttendance")]

        public async Task<IActionResult> captureAttendance(attendanceInfo data)
        {
            try
            {
                HttpClient client = new HttpClient();


                //client.BaseAddress = new Uri("http://192.168.247.22:5003");

                //client.BaseAddress = new Uri("http://192.168.247.22:5003");
                 client.BaseAddress = new Uri(" http://192.168.60.22:5003");

                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("Camera/camera/capture").Result;



                if (response.IsSuccessStatusCode)
                {
                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
                    // Convert this into base64
                    data.attnImage = Convert.ToBase64String(imageBytes);

                    data.date = DateTime.Now;

                    // string store in db



                    // user table 6 students present... default 0, each time +20

                    await insertAttn(data);

                    return File(imageBytes, "image/*");

                }

                else

                {

                    return BadRequest("Something went wrong");

                }

            }

            catch (Exception e)

            {

                return Ok(e.Message);

            }

        }



        #endregion
        #region Private Methods
        private async Task insertAttn(attendanceInfo data)

        {
            try
            {
                // Read from database of student User's Fname, Lname, rollno, dept, mailid, pmail attn based on dept 
                using (_context)

                {
                    var users = await _context.Users
                    .Where(u => u.Department == data.dept)
                    .Select(u => new
                    {
                        u.FirstName,
                        u.LastName,
                        u.RollNo,
                        u.Department,
                        u.Email,
                        u.ReferenceEmail,
                        u.Attendance

                    })

                    .ToListAsync();



                    // Existing attn + 20 add

                    foreach (var user in users)

                    {

                        //user.Attendance += 20;

                        //context.Entry(user).State = EntityState.Modified;

                    }



                    await _context.SaveChangesAsync();



                    // Create absentAlert for absent users and call sendAbsenceMailNotification



                    var presentStudents = users.Select(u => u.RollNo).ToList();

                    var allStudents = _context.Users

                    .Where(u => u.Role == "student" && u.Department == data.dept)

                    .Select(u => new

                    {
                        u.Id,
                        u.FirstName,
                        u.LastName,
                        u.RollNo,
                        u.Department,
                        u.Email,
                        u.ReferenceEmail,
                        u.Attendance

                        //}).Take(23)
                    }).ToList();

                    //.ToListAsync();

                    //var deptStudents = new List<Models.User>();
                    //var ps = presentStudents.Aggregate((a, b) => a + ", " + b);
                    //foreach (var usr in allStudents)
                    //{
                    //    if(ps.Contains(usr.RollNo))
                    //    {
                    //        deptStudents.Add(usr);
                    //    }
                    //}

                    //Func<student, bool> absentCondition = s => !presentStudents.Contains(s.RollNo);

                    //var absentStudents = allStudents.Where(absentCondition).ToList();



                    /*

                    var absentStudents = allStudents.Where(s => !presentStudents.Contains(s.RollNo)).ToList();

                    var absentStudentRecords = absentStudents.Select(s => new absenceMailNotification

                    {

                        FirstName = s.FirstName,

                        LastName = s.LastName,

                        StudentMail = s.Email,

                        Department = s.Department

                    }).ToList();

                    */

                    var absdata = new absenceAlert();

                    for (int count = 0; count < 5; count++)
                    {
                        var c = allStudents[count].Attendance;
                        c = allStudents[count].Attendance + 20;
                        //allStudents[count].Attendance = c;

                        var u = _context.Users.FirstOrDefault(u => u.Id == allStudents[count].Id);
                        if (u != null)
                        {
                            u.Attendance = c;
                            await _context.SaveChangesAsync();
                            //return Ok(u);

                        }



                    }


                    for (int count = 5; count < allStudents.Count; count++)

                    {

                        var std = new absenceMailNotification();

                        std.Department = allStudents[count].Department;

                        std.FirstName = allStudents[count].FirstName;

                        std.LastName = allStudents[count].LastName;

                        std.RollNo = allStudents[count].RollNo;

                        std.StudentMail = allStudents[count].Email;

                        std.ParentMail = allStudents[count].ReferenceEmail;

                        absdata.absentStudentRecords.Add(std);

                    }



                    /* 

                     absdata.absentStudentRecords = absentStudentRecords;

                         // Send email to all recipients

                    */

                    await sendAbsenceMailNotification(absdata);



                    // Warning: because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.

                }

                await _context.SaveChangesAsync();

            }



            catch (Exception e)

            {

                Console.WriteLine(e.Message);

            }
        }
        private async Task sendAbsenceMailNotification(absenceAlert records)

        {

            try

            {

                if (records.absentStudentRecords.Count > 0)

                {

                    for (int count = 0; count < records.absentStudentRecords.Count; count++)

                    {

                        await SendMail(records.absentStudentRecords[count]);

                        Task.Delay(1000);

                    }

                }

            }

            catch (Exception ex)

            {

                WriteLine("Exception while sending absent mail notifications.");

                WriteLine(ex);

            }

        }

        private async Task SendMail(absenceMailNotification student)

        {

            try

            {

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                smtpClient.UseDefaultCredentials = false; // uncomment if you don't want to use the network credentials

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
                //jjlfzyrhemrraklz
                smtpClient.Credentials = new System.Net.NetworkCredential("21mca113@charusat.edu.in", "jjlfzyrhemrraklz");

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtpClient.EnableSsl = true;

                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("21mca113@charusat.edu.in", "CHARUSAT ADMIN");

                // mail.To.Add(new MailAddress("anjalitank7729@gmail.com"));

                //mail.CC.Add(new MailAddress("hitarthirai14@gmail.com"));

                mail.To.Add(new MailAddress(student.ParentMail));

                mail.CC.Add(new MailAddress(student.StudentMail));

                mail.Subject = "Attendance Notification";

                mail.Body = student.FirstName + " " + student.LastName + " (" + student.RollNo + ") from dept " + student.Department + " is absent today.";

                smtpClient.Send(mail);

            }

            catch (Exception ex)

            {

                WriteLine("Exception while sending mail");

                WriteLine(ex);

            }

        }
        //private async Task WeeklyMail()
        //{
        //    using (_context)
        //    {
        //        var student = _context.Users

        //        .Where(u => u.Role == "student")

        //        .Select(u => new

        //        {
        //            u.Id,
        //            u.FirstName,
        //            u.LastName,
        //            u.RollNo,
        //            u.Department,
        //            u.Email,
        //            u.ReferenceEmail,
        //            u.Attendance

        //            //}).Take(23)
        //        }).ToList();
        //        foreach (var s in student)
        //        {
        //            _mailTimer = new MailTimer();

        //        }

        //      


        //    }
        #endregion

    }
}



