using System;
using System.Collections.Generic;

namespace Email.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// An equivalent to the ForEach for IList<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items is IEnumerable<T>)
            {
                foreach (var item in items)
                    action(item);
            }
        }
    }
}
