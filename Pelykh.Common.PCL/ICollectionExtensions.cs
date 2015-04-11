using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Pelykh.Common
{
    public static class ICollectionExtensions
    {
        #region Append
        public static ICollection<T> Append<T>(this ICollection<T> collection, T item)
        {
            collection.ThrowIfNull("this");

            collection.Add(item);
            return collection;
        }
        #endregion

        #region Remove
        public static bool Remove<T>(this ICollection<T> collection, Predicate<T> match)
        {
            collection.ThrowIfNull("this");
            match.ThrowIfNull("match");

            T matchedItem;
            try
            {
                matchedItem = collection.First(item => match(item));
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            return collection.Remove(matchedItem);
        }
        #endregion

        #region RemoveAll
        public static int RemoveAll<T>(this ICollection<T> collection, Predicate<T> match)
        {
            collection.ThrowIfNull("this");
            match.ThrowIfNull("match");

            int itemsRemoved = 0;
            foreach (var matchedItem in collection.Where(item => match(item)))
            {
                collection.Remove(matchedItem);
                itemsRemoved++;
            }

            return itemsRemoved;
        }
        #endregion
    }
}
