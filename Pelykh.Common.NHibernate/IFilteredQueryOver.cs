using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Pelykh.Common.NHibernate
{
    public interface IFilteredQueryOver<TEntity>
        where TEntity : class
    {
        IFilteredQueryOver<TEntity> FilterBy(PropertyProjection propertyProjection);
        IFilteredQueryOverFilterBuilder<TEntity> FilterBy(Expression<Func<TEntity, object>> memberExpression);
        IFilteredQueryOverFilterBuilder<TEntity> FilterBy(string property);
        
        IFilteredQueryOverNaturalIdFilterBuilder<TEntity> FilterByNaturalId { get; }

        IFilteredQueryOver<TEntity> Where(ICriterion criterion);
        IFilteredQueryOver<TEntity> Where(Expression<Func<TEntity, object>> memberExpression, ICriterion criterion);
        IFilteredQueryOver<TEntity> Where(string property, ICriterion criterion);

        IList<TEntity> List();
    }
}
