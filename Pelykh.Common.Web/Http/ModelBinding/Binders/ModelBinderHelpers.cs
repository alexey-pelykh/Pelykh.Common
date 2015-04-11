using System;
using System.Collections.Generic;
using System.Web.Http.ModelBinding;
namespace Pelykh.Common.Web.Http.ModelBinding.Binders
{
    public static class ModelBinderHelpers
    {
        public static string CreatePropertyModelName(string prefix, string propertyName)
        {
            if (string.IsNullOrEmpty(prefix))
                return propertyName ?? string.Empty;
            else if (string.IsNullOrEmpty(propertyName))
                return prefix ?? string.Empty;
            else
                return string.Format("{0}.{1}", prefix, propertyName);
        }

        public static void CreateOrReplaceCollection<TElement>(
            ModelBindingContext bindingContext,
            IEnumerable<TElement> incomingElements,
            Func<ICollection<TElement>> creator)
        {
            var collection = bindingContext.Model as ICollection<TElement>;
            if (collection == null || collection.IsReadOnly)
            {
                collection = creator();
                bindingContext.Model = collection;
            }

            collection.Clear();
            foreach (TElement element in incomingElements)
            {
                collection.Add(element);
            }
        }

        public static void CreateOrMergeCollection<TElement>(
            ModelBindingContext bindingContext,
            IEnumerable<TElement> incomingElements,
            Func<ICollection<TElement>> creator)
        {
            var collection = bindingContext.Model as ICollection<TElement>;
            if (collection == null || collection.IsReadOnly)
            {
                collection = creator();
                bindingContext.Model = collection;
            }

            foreach (TElement element in incomingElements)
            {
                collection.Add(element);
            }
        }

        public static void CreateOrReplaceDictionary<TKey, TValue>(
            ModelBindingContext bindingContext,
            IDictionary<TKey, TValue> incomingElements,
            Func<IDictionary<TKey, TValue>> creator)
        {
            var dictionary = bindingContext.Model as IDictionary<TKey, TValue>;
            if (dictionary == null || dictionary.IsReadOnly)
            {
                dictionary = creator();
                bindingContext.Model = dictionary;
            }

            dictionary.Clear();
            foreach (var element in incomingElements)
            {
                if (element.Key != null)
                {
                    dictionary[element.Key] = element.Value;
                }
            }
        }

        public static void CreateOrMergeDictionary<TKey, TValue>(
            ModelBindingContext bindingContext,
            IDictionary<TKey, TValue> incomingElements,
            Func<IDictionary<TKey, TValue>> creator)
        {
            var dictionary = bindingContext.Model as IDictionary<TKey, TValue>;
            if (dictionary == null || dictionary.IsReadOnly)
            {
                dictionary = creator();
                bindingContext.Model = dictionary;
            }

            foreach (var element in incomingElements)
            {
                if (element.Key != null)
                {
                    dictionary[element.Key] = element.Value;
                }
            }
        }
    }
}
