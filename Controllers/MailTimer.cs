using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Captureit.Data;
using Microsoft.EntityFrameworkCore;

namespace Captureit.Services
{
    public class MailTimer : IJob
    {
        private static ApplicationDbContext _context;
        public MailTimer()
        {

        }
        //public MailTimer(DbContextOptions<ApplicationDbContext> options)
        //{

        //    _context = new ApplicationDbContext(options);



        //}
        public MailTimer(ApplicationDbContext Context)
        {
            _context = Context;
        }


        public async Task Execute(IJobExecutionContext context)
        {
           // Console.WriteLine("bsjfjslfljk");
           
            
                var student = _context.Users
               .Where(u => u.Role == "student")

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


                foreach (var s in student)
                {

                    try
                    {
                        Console.WriteLine("Executing event");
                        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                        using (var client = new SmtpClient("smtp.gmail.com", 587))
                        {
                            client.UseDefaultCredentials = false;
                            client.Credentials = new NetworkCredential("21mca113@charusat.edu.in", "jjlfzyrhemrraklz");
                            var mail = new MailMessage();
                            client.EnableSsl = true;
                            mail.From = new MailAddress("harshmehta121992@gmail.com");
                            mail.To.Add(new MailAddress(s.Email));
                        
                            //mail.To.Add("khushbu.gupta1707@gmail.com");
                            mail.Subject = "Test mail";
                            mail.Body = "Dear " + s.FirstName +
                                    "Your weekly attendance is as follows:<br><br>" +
                                    "Attendance : " + s.Attendance +"%"+ "<br><br>" +
                                    "Thank you,<br>" +
                                    "CHARUSAT ADMIN"; 
                            mail.IsBodyHtml = true;

                            client.Send(mail);
                        }
                    var u = _context.Users.FirstOrDefault(u => u.Id == s.Id);
                        if(u != null)
                    {
                        u.Attendance = 0;
                        await _context.SaveChangesAsync();
                    }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }


                }
            }
        }

    }




