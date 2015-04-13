using Pelykh.Common;
using System;

namespace Pelykh.Common.HangFire
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class RecurringJobAttribute : Attribute
    {
        public string JobId { get; protected set; }
        public string CronExpression { get; protected set; }

        public RecurringJobAttribute(string jobId, string cronExpression)
        {
            jobId.ThrowIfNull("jobId");
            cronExpression.ThrowIfNull("cronExpression");

            JobId = jobId;
            CronExpression = cronExpression;
        }
    }
}
