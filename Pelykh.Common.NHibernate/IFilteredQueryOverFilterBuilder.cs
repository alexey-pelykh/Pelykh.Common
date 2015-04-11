using NHibernate.Criterion;

namespace Pelykh.Common.NHibernate
{
    public interface IFilteredQueryOverFilterBuilder<TEntity>
        where TEntity : class
    {
        IFilteredQueryOver<TEntity> As(IProjection projection);
    }
}
