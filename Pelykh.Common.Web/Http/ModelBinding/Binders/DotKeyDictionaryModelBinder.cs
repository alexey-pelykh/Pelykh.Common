using Pelykh.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using System.Web.Http.ValueProviders;

namespace Pelykh.Common.Web.Http.ModelBinding.Binders
{
    public class DotKeyDictionaryModelBinder<TKey, TValue> : DictionaryModelBinder<TKey, TValue>
    {
        public override bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var defaultModelBound = base.BindModel(actionContext, bindingContext);
            var dotKeyModelBound = BindDotKeyModel(actionContext, bindingContext);
            return defaultModelBound || dotKeyModelBound;
        }

        public bool BindDotKeyModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (!typeof(TKey).CanConvertFromString())
                return false;

            var valueProvider = bindingContext.ValueProvider as IEnumerableValueProvider;
            if (valueProvider == null)
                return false;

            var boundDictionary = new Dictionary<TKey, TValue>();
            var keyPrefix = bindingContext.ModelName + ".";
            var keyEntries = valueProvider.GetKeysFromPrefix(bindingContext.ModelName);
            foreach (var keyEntry in keyEntries)
            {
                var fullChildName = keyEntry.Value;
                if (!fullChildName.StartsWith(keyPrefix, StringComparison.Ordinal))
                    continue;

                var key = (TKey)TypeConversionHelpers.ConvertSimpleType(CultureInfo.CurrentCulture, keyEntry.Key, typeof(TKey));

                var childBindingContext = new ModelBindingContext(bindingContext)
                {
                    ModelMetadata = actionContext.GetMetadataProvider().GetMetadataForType(null, typeof(TValue)),
                    ModelName = fullChildName
                };
                if (!actionContext.Bind(childBindingContext))
                    continue;
                
                var boundValue = (TValue)childBindingContext.Model;

                bindingContext.ValidationNode.ChildNodes.Add(childBindingContext.ValidationNode);

                boundDictionary.Add(key, boundValue);
            }
            if (boundDictionary.Count == 0)
                return false;

            ModelBinderHelpers.CreateOrMergeDictionary(bindingContext, boundDictionary, () => new Dictionary<TKey, TValue>());
            return true;
        }
    }
}
