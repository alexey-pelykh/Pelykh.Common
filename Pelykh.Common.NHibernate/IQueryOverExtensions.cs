using NHibernate;
using NHibernate.SqlCommand;

namespace Pelykh.Common.NHibernate
{
    public static class IQueryOverExtensions
    {
        public static SqlString GenerateSql(this IQueryOver queryOver, ISession session = null)
        {
            queryOver.ThrowIfNull("this");

            return queryOver.RootCriteria.GenerateSql(session);
        }
    }
}
