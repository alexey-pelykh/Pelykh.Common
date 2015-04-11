using NHibernate.Criterion;
using System;
using System.Linq.Expressions;

namespace Pelykh.Common.NHibernate
{
    public interface IFilteredQueryOverNaturalIdFilterBuilder<TEntity> : IFilteredQueryOverFilterBuilder<TEntity>
        where TEntity : class
    {
        IFilteredQueryOver<TEntity> As(Func<string, IProjection> projection);

        IFilteredQueryOverNaturalIdFilterBuilder<TEntity> Without(params Expression<Func<TEntity, object>>[] memberExpressions);
        IFilteredQueryOverNaturalIdFilterBuilder<TEntity> Without(params string[] properties);
    }
}
