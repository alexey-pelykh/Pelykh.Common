﻿using Hangfire;
using System;
using System.Reflection;

namespace Pelykh.Common.HangFire
{
    public static class ScheduledJobs
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

        public sealed class Builder : JobsBuilder<Builder>
        {
            public Builder()
            {
            }

            public void Schedule()
            {
                ProcessJobMethods<ScheduledJobAttribute>(
                    (job) =>
                        BackgroundJob.Schedule(job.Expression, job.Attribute.EnqueueAt));
            }
        }
    }
}
