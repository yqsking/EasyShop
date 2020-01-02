using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyShop.Job.Jobs
{
    /// <summary>
    /// 定时任务管理
    /// </summary>
    public static  class JobManager
    {
        /// <summary>
        /// 调度定时任务
        /// </summary>
        public static  void Invoke()
        {
            ////队列任务（如未异常，只执行一次）
            //BackgroundJob.Enqueue(()=>Console.WriteLine("队列任务"));
            ////延迟任务（在延迟时间后，如未异常，只执行一次）
            //BackgroundJob.Schedule(()=>Console.WriteLine("延迟任务"),TimeSpan.FromDays(7));
            ////定时任务(UTC时间每天凌晨执行)
            //RecurringJob.AddOrUpdate(()=>Console.WriteLine("定时任务"),Cron.Daily);
         

        }
    }
}
