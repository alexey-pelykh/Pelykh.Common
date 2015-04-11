using NHibernate;
using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR;

namespace Pelykh.Common.NHibernate
{
    public static class IQueryExtensions
    {
        public static string GenerateSql(this IQuery query, ISession session)
        {
            query.ThrowIfNull("this");
            session.ThrowIfNull("session");

            var sessionImplementor = session.GetSessionImplementation();
            var translatorFactory = new ASTQueryTranslatorFactory();
            var translators = translatorFactory.CreateQueryTranslators(query.QueryString, null, false,
                sessionImplementor.EnabledFilters, sessionImplementor.Factory);

            return translators[0].SQLString;
        }
    }
}
