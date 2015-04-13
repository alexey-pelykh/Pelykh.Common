using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Pelykh.Common.HangFire
{
    public static class ManualJobs
    {
        #region Fluent builder creation
        public static Builder FromAssembly(Assembly assembly)
        {
            return new Builder().FromAssembly(assembly);
        }

        public static Builder FromAssemblies(params Assembly[] assemblies)
        {
            return new Builder().FromAssemblies(assemblies);
        }

        public static Builder FromAssemblyOf(Type type)
        {
            return new Builder().FromAssemblyOf(type);
        }

        public static Builder FromAssembliesOf(params Type[] types)
        {
            return new Builder().FromAssembliesOf(types);
        }

        public static Builder FromTypeName(string typeName)
        {
            return new Builder().FromTypeName(typeName);
        }

        public static Builder FromTypeNames(params string[] typeNames)
        {
            return new Builder().FromTypeNames(typeNames);
        }

        public static Builder FromType(Type type)
        {
            return new Builder().FromType(type);
        }

        public static Builder FromTypes(params Type[] types)
        {
            return new Builder().FromTypes(types);
        }
        #endregion

        private static void MaintainJobs(string collectionName, IEnumerable<string> typeNames)
        {
            ManualJobs.FromTypeNames(typeNames.ToArray()).AddOrUpdate(collectionName);
            RecurringJob.RemoveIfExists(string.Format("manualJobsMaintainance({0})", collectionName));
        }

        public sealed class Builder : JobsBuilder<Builder>
        {
            public Builder()
            {
            }

            public void AddOrUpdate(string collectionName = "default")
            {
                collectionName.ThrowIfNull("collectionName");

                var hourBefore = DateTime.UtcNow - TimeSpan.FromHours(1);
                var hourBeforeCronExpression = string.Format("0 12 {0} {1} *", hourBefore.Day, hourBefore.Month);

                var dayBefore = DateTime.UtcNow - TimeSpan.FromDays(1);
                var dayBeforeCronExpression = string.Format("0 12 {0} {1} *", dayBefore.Day, dayBefore.Month);

                var collection = ProcessJobMethods<ManualJobAttribute>(
                    (job) =>
                        RecurringJob.AddOrUpdate(job.Attribute.JobId, job.Expression, hourBeforeCronExpression));
                var types = collection.Select(x => x.Method.DeclaringType.AssemblyQualifiedName).ToList();

                RecurringJob.AddOrUpdate(
                    string.Format("manualJobsMaintainance({0})", collectionName),
                    () => MaintainJobs(collectionName, types),
                    dayBeforeCronExpression);
            }
        }
    }
}
