using NHibernate;
using NHibernate.Impl;
using NHibernate.Loader.Criteria;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using System;

namespace Pelykh.Common.NHibernate
{
    public static class ICriteriaExtensions
    {
        public static SqlString GenerateSql(
            this ICriteria criteria,
            ISession session = null,
            CriteriaQueryTranslator translator = null)
        {
            criteria.ThrowIfNull("this");

            var criteriaImpl = (CriteriaImpl)criteria;

            var sessionImplementor = criteriaImpl.Session;
            if (sessionImplementor == null && session != null)
                sessionImplementor = session.GetSessionImplementation();
            if (sessionImplementor == null)
                throw new InvalidOperationException("Criteria is detached from session and no session was specified");
            var sessionImpl = (SessionImpl)sessionImplementor;

            var factory = (SessionFactoryImpl)sessionImpl.SessionFactory;
            var implementors = factory.GetImplementors(criteriaImpl.EntityOrClassName);

            if (translator == null)
            {
                translator = new CriteriaQueryTranslator(
                    factory, 
                    criteriaImpl, 
                    criteriaImpl.EntityOrClassName, 
                    CriteriaQueryTranslator.RootSqlAlias);
            }

            CriteriaJoinWalker walker = new CriteriaJoinWalker((IOuterJoinLoadable)factory.GetEntityPersister(implementors[0]),
                translator, factory, criteriaImpl, criteriaImpl.EntityOrClassName, sessionImpl.EnabledFilters);

            return walker.SqlString;
        }
    }
}
