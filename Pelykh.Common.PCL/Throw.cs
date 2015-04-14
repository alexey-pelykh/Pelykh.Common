using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Pelykh.Common
{
    public static class Throw
    {
        public static void IfNull<T>(Expression<Func<T>> expression)
            where T : class
        {
            var body = (MemberExpression)expression.Body;
            var name = body.Member.Name;
            var value = ((FieldInfo)body.Member).GetValue(((ConstantExpression)body.Expression).Value);

            if (value != null)
                return;

            if (name != null)
                throw new ArgumentNullException(name);

            throw new ArgumentNullException();
        }
    }
}
