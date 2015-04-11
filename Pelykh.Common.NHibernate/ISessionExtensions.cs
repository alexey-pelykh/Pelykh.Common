using NHibernate;
using NHibernate.Criterion;
using System.Linq;

namespace Pelykh.Common.NHibernate
{
    public static class ISessionExtensions
    {
        public static void SaveOrUpdateByNaturalId(this ISession session, object obj)
        {
            session.ThrowIfNull("this");
            obj.ThrowIfNull("obj");

            var persistentClass = obj.GetType();
            var metadata = session.SessionFactory.GetClassMetadata(persistentClass);
            if (!metadata.HasNaturalIdentifier)
            {
                session.SaveOrUpdate(obj);
                return;
            }

            var criteria = session.CreateCriteria(persistentClass);
            var naturalIdCriterion = Restrictions.NaturalId();
            foreach (var naturalIdPropertyIndex in metadata.NaturalIdentifierProperties)
            {
                var propertyName = metadata.PropertyNames[naturalIdPropertyIndex];
                var propertyValue = metadata.GetPropertyValue(obj, propertyName, EntityMode.Poco);

                naturalIdCriterion = naturalIdCriterion.Set(propertyName, propertyValue);
            }
            var entityInDb = criteria
                .Add(naturalIdCriterion)
                .SetCacheable(true)
                .UniqueResult();

            if (entityInDb == null)
            {
                session.Save(obj);
                return;
            }

            for (var propertyIndex = 0; propertyIndex < metadata.PropertyNames.Length; propertyIndex++)
            {
                var propertyName = metadata.PropertyNames[propertyIndex];

                bool shouldSkip = !metadata.PropertyTypes[propertyIndex].IsMutable;
                if (metadata.HasIdentifierProperty)
                    shouldSkip = shouldSkip || (propertyName == metadata.IdentifierPropertyName);
                if (metadata.IsVersioned)
                    shouldSkip = shouldSkip || (propertyIndex == metadata.VersionProperty);
                shouldSkip = metadata.NaturalIdentifierProperties.Contains(propertyIndex);
                if (shouldSkip)
                    continue;

                var propertyValue = metadata.GetPropertyValue(obj, propertyName, EntityMode.Poco);
                metadata.SetPropertyValue(entityInDb, propertyName, propertyValue, EntityMode.Poco);
            }

            session.Update(entityInDb);
        }

        public static IFilteredQueryOver<TEntity> FilteredQueryOver<TEntity>(this ISession session)
            where TEntity : class
        {
            session.ThrowIfNull("this");

            return new FilteredQueryOver<TEntity>(session);
        }
    }
}
