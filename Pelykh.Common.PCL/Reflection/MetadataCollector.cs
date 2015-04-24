using System;
using System.Collections.Generic;
using System.Reflection;

namespace Pelykh.Common.Reflection
{
    public abstract class MetadataCollector<TCollector>
        where TCollector : MetadataCollector<TCollector>
    {
        readonly List<Type> collectedTypes = new List<Type>();

        public TCollector FromAssemblies(params Assembly[] assemblies)
        {
            assemblies.ThrowIfNull("assemblies");

            var collector = (TCollector)this;
            foreach (var assembly in assemblies)
                collector = collector.FromAssembly(assembly);

            return collector;
        }

        public TCollector FromAssembly(Assembly assembly)
        {
            assembly.ThrowIfNull("assembly");

            var collector = (TCollector)this;
            foreach (var type in assembly.GetTypes())
                collector = collector.FromType(type);

            return collector;
        }

        public TCollector FromAssembliesOf(params Type[] types)
        {
            types.ThrowIfNull("types");

            var collector = (TCollector)this;
            foreach (var type in types)
                collector = collector.FromAssembliesOf(type);

            return collector;
        }

        public TCollector FromAssemblyOf(Type type)
        {
            type.ThrowIfNull("type");

            return FromAssembly(type.Assembly);
        }

        public TCollector FromTypeNames(params string[] typeNames)
        {
            typeNames.ThrowIfNull("typeNames");

            var collector = (TCollector)this;
            foreach (var typeName in typeNames)
                collector = collector.FromTypeName(typeName);

            return collector;
        }

        public TCollector FromTypeName(string typeName)
        {
            typeName.ThrowIfNull("typeName");

            return FromType(Type.GetType(typeName));
        }
        
        public TCollector FromTypes(params Type[] types)
        {
            types.ThrowIfNull("types");

            var collector = (TCollector)this;
            foreach (var type in types)
                collector = collector.FromType(type);

            return collector;
        }

        public TCollector FromType(Type type)
        {
            type.ThrowIfNull("type");

            collectedTypes.Add(type);

            return (TCollector)this;
        }

        public List<Type> CollectTypes()
        {
            return new List<Type>(collectedTypes);
        }
    }
}
