using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Logic.StaticHelpers
{
    internal static class MemoryCacheExtensions
    {
        /// <summary>
        /// Retrieves the item from the cache where the result of <paramref name="getItemId"/>
        /// applied to this item equals <paramref name="id"/>.
        /// </summary>
        /// <typeparam name="TItem">Type of the item to retrieve</typeparam>
        /// <param name="cache">Cache to check in</param>
        /// <param name="id">ID of the item being searched for</param>
        /// <param name="getItemId">Method to get the required ID off the cached items</param>
        /// <param name="item">out parameter to return the found item</param>
        /// <returns>True if a matching item was found, false otherwise.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static bool TryRetrieveItemWithId<TItem>(this IMemoryCache cache, int id, Func<TItem, int> getItemId, out TItem item)
        {
            if (cache is null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            if (getItemId is null)
            {
                throw new ArgumentNullException(nameof(getItemId));
            }

            if (cache.TryGetValue<IEnumerable<TItem>>(GetItemListCacheKey<TItem>(), out var cachedList)
                && cachedList.Any(cachedItem => id == getItemId(cachedItem), out var result))
            {
                item = result;
                return true;
            }

            item = default;
            return false;
        }

        /// <summary>
        /// Retrieve all items of type <typeparamref name="TItem"/> from the cache.
        /// </summary>
        /// <typeparam name="TItem">Type of item to search for</typeparam>
        /// <param name="cache">Cache to check in</param>
        /// <param name="items">out parameter to return the found items</param>
        /// <returns>True if a matching item was found, false otherwise.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static bool TryRetrieveAllItems<TItem>(this IMemoryCache cache, out IEnumerable<TItem> items)
        {
            if (cache is null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            bool found = cache.TryRetrieveListOfAllItems<TItem>(out var itemsFromCache);
            items = itemsFromCache;
            return found;
        }

        /// <summary>
        /// Cache all the provided items.
        /// </summary>
        /// <typeparam name="TItem">Type of the items</typeparam>
        /// <param name="cache">The cache to use</param>
        /// <param name="items">Collection of items to add</param>
        /// <param name="priority">Priority for keeping the cached list in the cache</param>
        /// <exception cref="ArgumentNullException"></exception>
        internal static void CacheListOfItems<TItem>(this IMemoryCache cache, IEnumerable<TItem> items, CacheItemPriority priority)
        {
            if (cache is null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            TimeSpan expirationTime;
            switch (priority)
            {
                case CacheItemPriority.Low:
                    expirationTime = new TimeSpan(0, 1, 0);
                    break;
                case CacheItemPriority.Normal:
                    expirationTime = new TimeSpan(0, 10, 0);
                    break;
                case CacheItemPriority.High:
                    expirationTime = new TimeSpan(0, 0, 30);
                    break;
                case CacheItemPriority.NeverRemove:
                    expirationTime = new TimeSpan(1, 0, 0);
                    break;
                default:
                    throw new ArgumentException($"{nameof(CacheItemPriority)} value {priority} is not supported.", nameof(priority));
            }

            cache.Set(
                GetItemListCacheKey<TItem>(),
                items.ToList(),
                new MemoryCacheEntryOptions()
                {
                    Priority = priority,
                    AbsoluteExpirationRelativeToNow = expirationTime
                });
        }

        /// <summary>
        /// If a list of items of type <typeparamref name="TItem"/> is found, add <paramref name="item"/>.
        /// Else do nothing.
        /// </summary>
        /// <typeparam name="TItem">Type of the item</typeparam>
        /// <param name="cache">The cache to use</param>
        /// <param name="item">The item to add</param>
        internal static void AddItemToCachedList<TItem>(this IMemoryCache cache, TItem item)
        {
            if (cache is null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            if (cache.TryRetrieveListOfAllItems<TItem>(out var items))
            {
                items.Add(item);
            }
        }

        /// <summary>
        /// Removes the list of items with <typeparamref name="TItem"/> from the cache.
        /// If no such items are found, do nothing.
        /// </summary>
        /// <typeparam name="TItem">Type of items to be removed</typeparam>
        /// <param name="cache">Cache to remove the items from</param>
        /// <exception cref="ArgumentNullException"></exception>
        internal static void RemoveCachedListOfItems<TItem>(this IMemoryCache cache)
        {
            if (cache is null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            cache.Remove(GetItemListCacheKey<TItem>());
        }

        private static bool TryRetrieveListOfAllItems<TItem>(this IMemoryCache cache, out List<TItem> items)
        {
            if (cache is null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            if (cache.TryGetValue<List<TItem>>(GetItemListCacheKey<TItem>(), out var itemsFromCache))
            {
                items = itemsFromCache;
                return true;
            }

            items = default;
            return false;
        }

        private static string GetItemListCacheKey<TItem>()
        {
            return $"Item List, Type = {typeof(TItem).FullName}";
        }

        private static bool Any<T>(this IEnumerable<T> enumerable, Func<T, bool> condition, out T result)
        {
            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (condition is null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            foreach (var element in enumerable)
            {
                if (condition(element))
                {
                    result = element;
                    return true;
                }
            }

            result = default;
            return false;
        }
    }
}
