using System;
using System.Globalization;

namespace Pelykh.Common.HangFire
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ScheduledJobAttribute : Attribute
    {
        public DateTimeOffset EnqueueAt { get; protected set; }

        public ScheduledJobAttribute(TimeSpan delay)
            : this(new DateTimeOffset(DateTime.UtcNow, delay))
        {
        }

        public ScheduledJobAttribute(DateTimeOffset enqueueAt)
        {
            EnqueueAt = enqueueAt;
        }

        public ScheduledJobAttribute(string enqueueAt = null, string delay = null)
        {
            EnqueueAt = DateTimeOffset.Parse(enqueueAt, CultureInfo.InvariantCulture);
        }
    }
}
