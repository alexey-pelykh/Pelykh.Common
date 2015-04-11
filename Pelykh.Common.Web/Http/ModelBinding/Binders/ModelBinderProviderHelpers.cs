using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Pelykh.Common.Web.Http.ModelBinding.Binders
{
    public static class ModelBinderProviderHelpers
    {
        public static void InjectInteadOf<TOriginalProvider>(
            HttpConfiguration config,
            Func<TOriginalProvider, ModelBinderProvider> overridingProviderInflater)
            where TOriginalProvider : ModelBinderProvider
        {
            var alteredServices = config.Services.GetServices(typeof(ModelBinderProvider))
                .ToList()
                .ReplaceOne(
                    x => x.GetType() == typeof(TOriginalProvider),
                    x => overridingProviderInflater((TOriginalProvider)x));
            config.Services.ReplaceRange(typeof(ModelBinderProvider), alteredServices);
        }

        public static IModelBinder GetGenericBinder(Type interfaceType, Type instanceType, Type openBinderType, Type modelType)
        {
            var modelTypeArguments = GetGenericBinderTypeArgs(interfaceType, modelType);
            if (modelTypeArguments == null)
                return null;

            var closedInstanceType = instanceType.MakeGenericType(modelTypeArguments);
            if (!modelType.IsAssignableFrom(closedInstanceType))
                return null;

            var closedBinderType = openBinderType.MakeGenericType(modelTypeArguments);
            var binder = (IModelBinder)Activator.CreateInstance(closedBinderType);
            return binder;
        }

        public static Type[] GetGenericBinderTypeArgs(Type interfaceType, Type modelType)
        {
            if (!modelType.IsGenericType || modelType.IsGenericTypeDefinition)
                return null;

            var modelTypeArguments = modelType.GetGenericArguments();
            if (modelTypeArguments.Length != interfaceType.GetGenericArguments().Length)
                return null;

            return modelTypeArguments;
        }
    }
}
