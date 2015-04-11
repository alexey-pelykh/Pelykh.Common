using NHibernate.Criterion;

namespace Pelykh.Common.NHibernate
{
    internal sealed class FilteredQueryOverFilterBuilder<TEntity> : IFilteredQueryOverFilterBuilder<TEntity>
        where TEntity : class
    {
        private readonly FilteredQueryOver<TEntity> owner;
        private readonly string property;

        public FilteredQueryOverFilterBuilder(FilteredQueryOver<TEntity> owner, string property)
        {
            owner.ThrowIfNull("owner");
            property.ThrowIfNull("property");

            this.owner = owner;
            this.property = property;
        }

        public IFilteredQueryOver<TEntity> As(IProjection projection)
        {
            projection.ThrowIfNull("projection");

            owner.AddFilter(property, projection);
            return owner;
        }
    }
}
