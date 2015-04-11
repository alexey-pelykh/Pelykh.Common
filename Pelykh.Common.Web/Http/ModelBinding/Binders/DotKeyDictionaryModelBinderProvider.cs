using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;

namespace Pelykh.Common.Web.Http.ModelBinding.Binders
{
    public class DotKeyDictionaryModelBinderProvider : BaseOverridingModelBinderProvider<DictionaryModelBinderProvider>
    {
        public DotKeyDictionaryModelBinderProvider(DictionaryModelBinderProvider originalProvider)
            : base(originalProvider)
        {
        }

        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
        {
            var overridden = ModelBinderProviderHelpers.GetGenericBinder(
                typeof(IDictionary<,>),
                typeof(Dictionary<,>),
                typeof(DotKeyDictionaryModelBinder<,>),
                modelType);
            return overridden ?? OriginalProvider.GetBinder(configuration, modelType);
        }

        public static void InjectInto(HttpConfiguration config)
        {
            ModelBinderProviderHelpers.InjectInteadOf<DictionaryModelBinderProvider>(
                config,
                x => new DotKeyDictionaryModelBinderProvider(x));
        }
    }
}
