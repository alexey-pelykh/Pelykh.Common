using NHibernate.Criterion;
using System;
using System.Linq;

namespace Pelykh.Common.NHibernate
{
    public static class ICriterionExtensions
    {
        public static string GetPropertyName(this ICriterion criterion)
        {
            criterion.ThrowIfNull("this");

            if (criterion is PropertySubqueryExpression)
                return ((PropertySubqueryExpression)criterion).GetPropertyName();

            var projections = criterion.GetProjections();
            if (projections == null)
                throw new InvalidOperationException(string.Format("Can not determine property name from criterion"));
            var projection = criterion.GetProjections().FirstOrDefault();
            if (projection == null)
                throw new InvalidOperationException(string.Format("Can not determine property name from criterion"));
            var propertyProjection = projection as IPropertyProjection;
            if (propertyProjection == null)
                throw new InvalidOperationException(string.Format("Can not determine property name from criterion"));
            return propertyProjection.PropertyName;
        }
    }
}
