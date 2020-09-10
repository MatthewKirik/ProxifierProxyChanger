using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AutoProxyChanger
{
    class Program
    {
        private static IScheduler scheduler;
        static void Main(string[] args)
        {
            NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
            StdSchedulerFactory factory = new StdSchedulerFactory(props);
            scheduler = factory.GetScheduler().GetAwaiter().GetResult();
            scheduler.Start().GetAwaiter().GetResult();

            IJobDetail job = JobBuilder.Create<ChangeProxyJob>()
                .WithIdentity(Guid.NewGuid().ToString(), "group2")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(Guid.NewGuid().ToString(), "group2")
                .StartNow()
                //.StartAt(DateTime.Today.AddDays(1).AddMinutes(double.Parse(ConfigurationManager.AppSettings["midnightOffset"])))
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(3)
                    //.WithIntervalInSeconds(20)
                    .RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger).GetAwaiter().GetResult();

            while (true)
            {
                Console.ReadKey(false);
            }
        }
    }
}
