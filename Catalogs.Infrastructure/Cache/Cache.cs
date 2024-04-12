using Newtonsoft.Json;
using System.Text;

namespace Catalogs.Infrastructure.Cache
{
    internal static class Cache<T>
    {
        public static T? GetDataFromCache(byte[] cache)
        {
            var serializedData = Encoding.UTF8.GetString(cache);
            var data = JsonConvert.DeserializeObject<T>(serializedData);

            return data;
        }

        public static byte[] ConvertDataForCaching(T data, out DistributedCacheEntryOptions options)
        {
            var serializedData = JsonConvert.SerializeObject(data);

            var bytedData = Encoding.UTF8.GetBytes(serializedData);

            options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10));

            return bytedData;
        }
    }
}