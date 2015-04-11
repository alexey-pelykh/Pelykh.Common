using NHibernate.Criterion;
using System.Reflection;

namespace Pelykh.Common.NHibernate
{
    public static class PropertySubqueryExpressionExtensions
    {
        private static readonly FieldInfo propertyField;

        static PropertySubqueryExpressionExtensions()
        {
            propertyField =
                typeof(PropertySubqueryExpression).GetField("propertyName", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static string GetPropertyName(this PropertySubqueryExpression propertySubqueryExpression)
        {
            propertySubqueryExpression.ThrowIfNull("this");

            return (string)propertyField.GetValue(propertySubqueryExpression);
        }
    }
}
