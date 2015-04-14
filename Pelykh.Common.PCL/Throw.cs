using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Pelykh.Common
{
    public static class Throw
    {
        public static Validator<T> If<T>(Expression<Func<T>> parameterExpression)
            where T : class
        {
            return new Validator<T>(parameterExpression);
        }

        public static void IfNull<T>(Expression<Func<T>> parameterExpression)
            where T : class
        {
            new Validator<T>(parameterExpression).IsNull();
        }

        public sealed class Validator<T>
            where T : class
        {
            public Expression<Func<T>> ParameterExpression { get; set; }

            public Validator(Expression<Func<T>> parameterExpression)
            {
                ParameterExpression = parameterExpression;
            }

            public void IsNull()
            {
                var body = (MemberExpression)ParameterExpression.Body;
                var name = body.Member.Name;
                var value = ((FieldInfo)body.Member).GetValue(((ConstantExpression)body.Expression).Value);

                if (value != null)
                    return;

                throw new ArgumentNullException(name);
            }
        }
    }
}
