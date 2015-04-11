using System;
using System.Collections.Generic;

namespace Pelykh.Common
{
    public static class IListExtensions
    {
        #region Replace
        public static IList<T> Replace<T>(this IList<T> list, T currentItem, T newItem)
        {
            list.ThrowIfNull("this");

            var itemIndex = list.IndexOf(currentItem);
            list[itemIndex] = newItem;
            return list;
        }
        #endregion

        #region ReplaceAll
        public static IList<T> ReplaceAll<T>(this IList<T> list, Predicate<T> itemSelector, Func<T, T> newItemGenerator)
        {
            list.ThrowIfNull("this");
            itemSelector.ThrowIfNull("itemSelector");
            newItemGenerator.ThrowIfNull("newItemGenerator");

            for (int index = 0; index < list.Count; index++)
            {
                var currentItem = list[index];
                if (!itemSelector(currentItem))
                    continue;

                list[index] = newItemGenerator(currentItem);
            }
            return list;
        }
        #endregion

        #region ReplaceOne
        public static IList<T> ReplaceOne<T>(this IList<T> list, Predicate<T> itemSelector, Func<T, T> newItemGenerator)
        {
            list.ThrowIfNull("this");
            itemSelector.ThrowIfNull("itemSelector");
            newItemGenerator.ThrowIfNull("newItemGenerator");

            for (int index = 0; index < list.Count; index++)
            {
                var currentItem = list[index];
                if (!itemSelector(currentItem))
                    continue;

                list[index] = newItemGenerator(currentItem);
                break;
            }
            return list;
        }
        #endregion
    }
}
