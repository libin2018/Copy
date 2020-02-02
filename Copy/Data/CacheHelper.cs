using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Extensions.Caching.Memory;

namespace Copy
{
    public class CacheHelper
    {
        private static readonly MemoryCache Cache = new MemoryCache(new MemoryCacheOptions());

        /// <summary>
        /// 获取缓存中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static object GetCacheValue(string key)
        {
            if (!string.IsNullOrEmpty(key) && Cache.TryGetValue(key, out var val))
            {
                return val;
            }
            return default(object);
        }

        /// <summary>
        /// 阅后即焚
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void RemoveCache(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void SetCacheValue(string key, object value, int min)
        {
            if (!string.IsNullOrEmpty(key))
            {
                Cache.Set(key, value, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(min)
                });
            }
        }
    }
}