using NHibernate.Criterion;
using NHibernate.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Pelykh.Common.NHibernate
{
    internal sealed class FilteredQueryOverNaturalIdFilterBuilder<TEntity> : IFilteredQueryOverNaturalIdFilterBuilder<TEntity>
        where TEntity : class
    {
        private readonly FilteredQueryOver<TEntity> owner;
        private readonly IEnumerable<string> naturalIdProperties;
        private readonly ISet<string> excludedProperties = new HashSet<string>();

        public FilteredQueryOverNaturalIdFilterBuilder(FilteredQueryOver<TEntity> owner, IEnumerable<string> naturalIdProperties)
        {
            owner.ThrowIfNull("owner");
            naturalIdProperties.ThrowIfNull("naturalIdProperties");

            this.owner = owner;
            this.naturalIdProperties = naturalIdProperties;
        }

        public IFilteredQueryOver<TEntity> As(Func<string, IProjection> projection)
        {
            projection.ThrowIfNull("projection");

            foreach (var property in naturalIdProperties)
            {
                if (excludedProperties.Contains(property))
                    continue;

                owner.AddFilter(property, projection(property));
            }
            return owner;
        }

        public IFilteredQueryOver<TEntity> As(IProjection projection)
        {
            projection.ThrowIfNull("projection");

            return As(x => projection);
        }

        public IFilteredQueryOverNaturalIdFilterBuilder<TEntity> Without(
            params Expression<Func<TEntity, object>>[] memberExpressions)
        {
            memberExpressions.ThrowIfNull("memberExpressions");

            var properties = memberExpressions.Select(x => ExpressionProcessor.FindMemberExpression(x.Body)).ToArray();
            return Without(properties);
        }

        public IFilteredQueryOverNaturalIdFilterBuilder<TEntity> Without(params string[] properties)
        {
            properties.ThrowIfNull("properties");

            foreach (var property in properties)
                excludedProperties.Add(property);
            return this;
        }
    }
}
