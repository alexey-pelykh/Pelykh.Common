using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;
using System;
using System.Linq.Expressions;

namespace Pelykh.Common.NHibernate.Mapping
{
    public static class OneToManyPartExtensions
    {
        public static OneToManyPart<TChild> KeyColumn<TChild, TParent>(this OneToManyPart<TChild> oneToManyPart,
            Expression<Func<TChild, TParent>> memberExpression)
        {
            return oneToManyPart.KeyColumn(memberExpression.ToMember().Name + "_id");
        }
    }
}
