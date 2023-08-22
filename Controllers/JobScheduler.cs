using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace Captureit.Services
{
    public class JobScheduler
    {
        //private static string cronJob = "0 0 0 ? * SAT *";
        private static string cronJob = "0 30 15 ? * THU *";
        private static bool reschedule_event = false;

        public async Task Reschedule(int hr)
        {
            cronJob = "0 0 " + hr.ToString() + " ? * FRI *";
            reschedule_event = true;
            await Start();
        }

        public static async Task Start()
        {
            try
            {
                Console.WriteLine(cronJob);
                IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
                await scheduler.Start();

                IJobDetail job = JobBuilder.Create<MailTimer>().WithIdentity("mail", "Mailtimer").Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("MailTimer", "Mailtimer")
                    .WithDescription("Mailtimer")
                    .StartNow()
                    .WithCronSchedule(cronJob)
                    .Build();

                if (reschedule_event)
                {
                    trigger = TriggerBuilder.Create()
                    .WithIdentity("MailTimer", "Mailtimer")
                    .WithDescription("Mailtimer")
                    .StartNow()
                    .WithCronSchedule(cronJob)
                    .Build();

                    await scheduler.RescheduleJob(new TriggerKey("MailTimer", "Mailtimer"), trigger);
                }
                else
                {
                    await scheduler.ScheduleJob(job, trigger);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }
}
