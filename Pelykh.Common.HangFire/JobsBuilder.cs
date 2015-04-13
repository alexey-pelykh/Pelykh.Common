using Pelykh.Common.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Pelykh.Common.HangFire
{
    public abstract class JobsBuilder<TBuilder> : MetadataCollector<TBuilder>
        where TBuilder : JobsBuilder<TBuilder>
    {
        internal JobsBuilder()
        {
        }

        protected IEnumerable<Job<TAttribute>> ProcessJobMethods<TAttribute>(Action<Job<TAttribute>> action)
            where TAttribute : Attribute
        {
            action.ThrowIfNull("action");

            var jobMethodBinding =
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy |
                BindingFlags.InvokeMethod;

            var methods = CollectTypes()
                .Select(type => type.GetMethods(jobMethodBinding).ToList())
                .Aggregate((a, b) => a.Concat(b).ToList())
                .Where(method =>
                    method.GetParameters().Length == 0 &&
                    !method.ContainsGenericParameters &&
                    method.GetCustomAttribute<TAttribute>() != null &&
                    (method.IsStatic || method.DeclaringType.GetConstructor(Type.EmptyTypes) != null));

            var jobs = new List<Job<TAttribute>>();
            foreach (var method in methods)
            {
                var expression = method.IsStatic
                    ? Expression.Lambda<Action>(Expression.Call(method))
                    : Expression.Lambda<Action>(Expression.Call(Expression.New(method.DeclaringType), method.Name, null));
                var attribute = method.GetCustomAttribute<TAttribute>();

                var job = new Job<TAttribute>()
                {
                    Method = method,
                    Expression = expression,
                    Attribute = attribute
                };
             
                action(job);

                jobs.Add(job);
            }

            return jobs;
        }

        public class Job<TAttribute>
            where TAttribute : Attribute
        {
            public MethodInfo Method { get; set; }
            public Expression<Action> Expression { get; set; }
            public TAttribute Attribute { get; set; }
        }
    }
}
