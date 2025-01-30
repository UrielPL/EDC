using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CatalogosLTH.Web
{
    public class JobScheduler
    {
        public static void Start()
        {
            //IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

            //scheduler.Start();

            //IJobDetail job = JobBuilder.Create<CorteMensualJob>().Build();

            //ITrigger trigger = TriggerBuilder.Create()
            //         .WithIdentity("CorteMensualJob", "IDG")
            //         .WithCronSchedule("0 0/1 * 1/1 * ? *")
            //         .StartNow()
            //         //.StartAt(DateTime.Now)
            //         .WithPriority(1)
            //         .Build();

            // ITrigger trigger = TriggerBuilder.Create()

            // .WithIdentity("VKJob", "IDG")

            //.StartNow()

            //.WithSimpleSchedule(s => s

            //    .WithIntervalInSeconds(10)

            //    .RepeatForever())

            //.Build();

            //scheduler.ScheduleJob(job, trigger);
        }
    }
}