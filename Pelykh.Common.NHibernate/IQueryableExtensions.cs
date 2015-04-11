using NHibernate;
using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Linq;
using System.Linq;

namespace Pelykh.Common.NHibernate
{
    public static class IQueryableExtensions
    {
        public static string GenerateSql(this IQueryable queryable, ISession session)
        {
            queryable.ThrowIfNull("this");
            session.ThrowIfNull("session");

            var sessionImplementor = session.GetSessionImplementation();
            var nhLinqExpression = new NhLinqExpression(queryable.Expression, sessionImplementor.Factory);
            var translatorFactory = new ASTQueryTranslatorFactory();
            var translators = translatorFactory.CreateQueryTranslators(nhLinqExpression, null, false,
                sessionImplementor.EnabledFilters, sessionImplementor.Factory);

            return translators[0].SQLString;
        }
    }
}
