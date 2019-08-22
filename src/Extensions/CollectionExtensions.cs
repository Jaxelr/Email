using System.Collections.Generic;
using System.Linq;

namespace EmailService.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Since the mapping from the DI returns an instantiated class as based on the appsettings.json
        /// config file, we must check if the only element included is not empty, then we can define it as empty.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        internal static bool Empty(this ICollection<string> collection) =>
            (collection.Count == 1 && string.IsNullOrEmpty(collection.FirstOrDefault())) ? true : false;
    }
}
