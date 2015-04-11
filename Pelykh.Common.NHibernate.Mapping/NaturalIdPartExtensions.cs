using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Pelykh.Common.NHibernate.Mapping
{
    public static class NaturalIdPartExtensions
    {
        private static IList<PropertyMapping> GetProperties<T>(this NaturalIdPart<T> naturalIdPart)
        {
            naturalIdPart.ThrowIfNull("this");

            var propertiesField = naturalIdPart.GetType().GetField("properties", BindingFlags.Instance | BindingFlags.NonPublic);
            return (IList<PropertyMapping>)propertiesField.GetValue(naturalIdPart);
        }

        public static NaturalIdPart<T> Property<T>(
            this NaturalIdPart<T> naturalIdPart,
            Expression<Func<T, object>> expression,
            Action<PropertyPart> propertyAction)
        {
            naturalIdPart.ThrowIfNull("this");
            expression.ThrowIfNull("expression");
            propertyAction.ThrowIfNull("propertyAction");

            var propertyPart = new PropertyPart(expression.ToMember(), typeof(T));

            propertyAction(propertyPart);
            naturalIdPart.GetProperties().Add((propertyPart as IPropertyMappingProvider).GetPropertyMapping());

            return naturalIdPart;
        }

        private static IList<ManyToOneMapping> GetManyToOnes<T>(this NaturalIdPart<T> naturalIdPart)
        {
            naturalIdPart.ThrowIfNull("this");

            var manyToOnesField = naturalIdPart.GetType().GetField("manyToOnes", BindingFlags.Instance | BindingFlags.NonPublic);
            return (IList<ManyToOneMapping>)manyToOnesField.GetValue(naturalIdPart);
        }

        public static NaturalIdPart<T> Reference<T, TOther>(
            this NaturalIdPart<T> naturalIdPart,
            Expression<Func<T, TOther>> expression,
            Action<ManyToOnePart<TOther>> manyToOneAction)
        {
            naturalIdPart.ThrowIfNull("this");
            expression.ThrowIfNull("expression");
            manyToOneAction.ThrowIfNull("manyToOneAction");

            var manyToOnePart = new ManyToOnePart<TOther>(typeof(T), expression.ToMember());
            
            manyToOneAction(manyToOnePart);
            naturalIdPart.GetManyToOnes().Add((manyToOnePart as IManyToOneMappingProvider).GetManyToOneMapping());

            return naturalIdPart;
        }
    }
}
