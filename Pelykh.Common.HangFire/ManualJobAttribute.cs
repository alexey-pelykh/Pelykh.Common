using Pelykh.Common;
using System;

namespace Pelykh.Common.HangFire
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ManualJobAttribute : Attribute
    {
        public string JobId { get; protected set; }

        public ManualJobAttribute(string jobId)
        {
            jobId.ThrowIfNull("jobId");

            JobId = jobId;
        }
    }
}
