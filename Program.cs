using Captureit.Data;
using Captureit.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Captureit
{
    
    public class Program
    {
        private static MailTimer _mailTimer;
        private static JobScheduler jobScheduler;
      // private ApplicationDbContext _context;
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var scope = host.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureCreated();
            jobScheduler = new JobScheduler();
            _mailTimer = new MailTimer(db);
            //using (var scope = host.Services.CreateScope())
            //{
            //    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //    db.Database.EnsureCreated();
            //    jobScheduler = new JobScheduler();
            //    _mailTimer = new MailTimer(db);
            //}
            // _mailTimer = new MailTimer();

            JobScheduler.Start().GetAwaiter().GetResult();
           // CreateHostBuilder(args).Build();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
      
    }
}
//Changed Code 
// DbContext context = host.Services.GetService<DbContext>();

//ApplicationDbContext context1 = host.Services.GetServices<ApplicationDbContext>;
// var  = host.Services.GetService<DbContext>();
// DbContext context = Host.CreateDefaultBuilder





//For Normal Execution
//jobScheduler = new JobScheduler();
//_mailTimer = new MailTimer();

//JobScheduler.Start().GetAwaiter().GetResult();
//CreateHostBuilder(args).Build().Run();