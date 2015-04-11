using NHibernate;
using NHibernate.Criterion;
using NHibernate.Impl;
using NHibernate.Loader.Criteria;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Pelykh.Common.NHibernate
{
    internal sealed class FilteredQueryOver<TEntity> : IFilteredQueryOver<TEntity>
        where TEntity : class
    {
        public ISession Session { get; private set; }

        private readonly SingleTableEntityPersister entityPersister;
        private readonly List<string> naturalIdProperties;
        private readonly Dictionary<string, IProjection> projections = new Dictionary<string, IProjection>();
        private readonly Dictionary<string, ICriterion> criterions = new Dictionary<string, ICriterion>();
        private readonly Dictionary<string, List<string>> selectedPropertyToColumnAliasMap =
            new Dictionary<string, List<string>>();
        private readonly SqlString baseSelectSql;

        public FilteredQueryOver(ISession session)
        {
            session.ThrowIfNull("session");

            Session = session;
            var metadata = Session.SessionFactory.GetClassMetadata(typeof(TEntity));
            entityPersister = metadata as SingleTableEntityPersister;
            naturalIdProperties = metadata.NaturalIdentifierProperties.Select(x => metadata.PropertyNames[x]).ToList();
            var criteria = (CriteriaImpl)Session.CreateCriteria<TEntity>();
            var selectTranslator = new CriteriaQueryTranslator(Session.GetSessionImplementation().Factory, criteria,
                criteria.EntityOrClassName, "_Data");
            foreach (var property in metadata.PropertyNames)
            {
                selectedPropertyToColumnAliasMap[property] =
                    selectTranslator.GetColumnsUsingProjection(criteria, property).ToList();
            }
            baseSelectSql = criteria.GenerateSql(translator: selectTranslator);
        }

        public IFilteredQueryOver<TEntity> FilterBy(PropertyProjection propertyProjection)
        {
            propertyProjection.ThrowIfNull("propertyProjection");

            AddFilter(propertyProjection.PropertyName, propertyProjection);
            return this;
        }

        public IFilteredQueryOverFilterBuilder<TEntity> FilterBy(Expression<Func<TEntity, object>> memberExpression)
        {
            memberExpression.ThrowIfNull("memberExpression");

            var property = ExpressionProcessor.FindMemberExpression(memberExpression.Body);
            return FilterBy(property);
        }

        public IFilteredQueryOverFilterBuilder<TEntity> FilterBy(string property)
        {
            property.ThrowIfNull("property");

            return new FilteredQueryOverFilterBuilder<TEntity>(this, property);
        }

        public IFilteredQueryOverNaturalIdFilterBuilder<TEntity> FilterByNaturalId
        {
            get
            {
                if (!naturalIdProperties.Any())
                    throw new InvalidOperationException(string.Format("{0} does not have natural identifier", typeof(TEntity)));
                return new FilteredQueryOverNaturalIdFilterBuilder<TEntity>(this, naturalIdProperties);
            }
        }

        public IFilteredQueryOver<TEntity> Where(ICriterion criterion)
        {
            criterion.ThrowIfNull("criterion");

            return Where(criterion.GetPropertyName(), criterion);
        }

        public IFilteredQueryOver<TEntity> Where(Expression<Func<TEntity, object>> memberExpression, ICriterion criterion)
        {
            memberExpression.ThrowIfNull("memberExpression");
            criterion.ThrowIfNull("criterion");

            var property = ExpressionProcessor.FindMemberExpression(memberExpression.Body);
            return Where(property, criterion);
        }

        public IFilteredQueryOver<TEntity> Where(string property, ICriterion criterion)
        {
            property.ThrowIfNull("property");
            criterion.ThrowIfNull("criterion");

            criterions[property] = criterion;
            return this;
        }

        public IList<TEntity> List()
        {
            var query = CompileQuery();
            query.AddEntity(typeof(TEntity));
            return query.List<TEntity>();
        }

        internal void AddFilter(string property, IProjection projection)
        {
            property.ThrowIfNull("property");
            projection.ThrowIfNull("projection");

            if (!projection.IsGrouped && !projection.IsAggregate)
                throw new ArgumentException("Projection has to be grouped or aggregate", "projection");

            projections[property] = projection;
        }

        private ISQLQuery CompileQuery()
        {
            var subcriteria = (CriteriaImpl)Session.CreateCriteria<TEntity>(); ;
            var projectionList = Projections.ProjectionList();
            foreach (var projection in projections)
                projectionList.Add(projection.Value, projection.Key);
            subcriteria.SetProjection(projectionList);
            foreach (var criterion in criterions)
                subcriteria.Add(criterion.Value);

            var subqueryTranslator = new CriteriaQueryTranslator(Session.GetSessionImplementation().Factory, subcriteria,
                subcriteria.EntityOrClassName, "_InnerFilter");

            var filterPropertyToColumnAliasMap = new Dictionary<string, List<string>>();
            foreach (var projection in projections)
            {
                filterPropertyToColumnAliasMap[projection.Key] =
                    subqueryTranslator.GetColumnAliasesUsingProjection(subcriteria, projection.Key).ToList();
            }

            var joinOnMap = new Dictionary<string, string>();
            foreach (var projection in projections)
            {
                var selectColumns = selectedPropertyToColumnAliasMap[projection.Key];
                var filterColumns = filterPropertyToColumnAliasMap[projection.Key];
                var columnsCount = Math.Min(selectColumns.Count, filterColumns.Count);
                for (int columnIndex = 0; columnIndex < columnsCount; columnIndex++)
                    joinOnMap[selectColumns[columnIndex]] = filterColumns[columnIndex];
            }
            var subcriteriaSqlString = subcriteria.GenerateSql(translator: subqueryTranslator);

            var queryBuilder = new StringBuilder();

            //TODO: Why this does not work: NHibernate tries to resolve "{PropertyName}", but result contains some odd mangled name
            //queryBuilder.AppendLine(baseSelectSql);
            queryBuilder.AppendLine("SELECT _Data.* FROM " + entityPersister.TableName + " AS _Data");
            queryBuilder.AppendLine("INNER JOIN (");
            queryBuilder.Append("\t");
            queryBuilder.AppendLine(subcriteriaSqlString.ToString());
            queryBuilder.AppendLine(") AS _Filter");
            queryBuilder.AppendLine("ON");
            queryBuilder.AppendLine(string.Join(" AND \n",
                joinOnMap.Select(x => string.Format("\t{0} = _Filter.{1}", x.Key, x.Value))));

            var sqlQuery = Session.CreateSQLQuery(queryBuilder.ToString());

            var parameterEnumerator = subqueryTranslator.CollectedParameters.GetEnumerator();
            var parameterSpecificationEnumerator = subqueryTranslator.CollectedParameterSpecifications.GetEnumerator();
            for (var parameterIndex = 0; parameterIndex < subcriteriaSqlString.GetParameterCount(); parameterIndex++)
            {
                parameterEnumerator.MoveNext();
                parameterSpecificationEnumerator.MoveNext();

                var parameter = parameterEnumerator.Current;
                var parameterSpecification = parameterSpecificationEnumerator.Current;
                sqlQuery.SetParameter(parameterIndex, parameter.Value, parameter.Type);
            }

            return sqlQuery;
        }
    }
}
