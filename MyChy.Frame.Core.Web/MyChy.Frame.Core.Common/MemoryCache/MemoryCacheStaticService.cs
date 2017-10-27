using Microsoft.Extensions.Caching.Memory;
using MyChy.Frame.Core.Common.Extensions;
using MyChy.Frame.Core.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyChy.Frame.Core.Common.MemoryCache
{
    public class MemoryCacheStaticService
    {
        protected static IMemoryCache Cache;
        private static MemoryCacheConfig _config = null;
        public static bool IsCache=false;


        static MemoryCacheStaticService()
        {
            if (_config != null) return;
            var config = new ConfigHelper();
            _config = config.Reader<MemoryCacheConfig>("config/MemoryCache.json");
            if (_config == null || _config.Second == 0)
            {
                _config = new MemoryCacheConfig { IsCache = false };
            }
            IsCache = _config.IsCache;
            var cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions());
            Cache = cache;
        }


        /// <summary>
        /// 验证缓存项是否存在
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            if (!IsCache) return false;
            if (key == null)
            {
                return false;
                // throw new ArgumentNullException(nameof(key));
            }
            return Cache.TryGetValue(key, out object cached);
        }

        #region 添加缓存

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <returns></returns>
        public static void Set(string key, object value)
        {
            Set(key, value, _config.Second);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="second">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        public static void Set(string key, object value, int second)
        {
            Set(key, value, second, DateTime.Now.AddDays(1).Date);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="second">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="expiressTime">绝对过期时长</param>
        public static void Set(string key, object value, int second, DateTime expiressTime)
        {
            var expiresSliding = DateTime.Now.AddSeconds(second) - DateTime.Now;
            var expiressAbsoulte = expiressTime - DateTime.Now;
            Set(key, value, expiresSliding, expiressAbsoulte);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        private static void Set(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (!IsCache) return;
            if (key == null || value == null)
            {
                return;
            }
            Cache.Set(key, value,
                new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(expiresSliding)
                    .SetAbsoluteExpiration(expiressAbsoulte)
                );
        }

        #endregion

        #region 删除缓存

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static void Remove(string key)
        {
            if (!IsCache) return;
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            Cache.Remove(key);
        }

        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        /// <returns></returns>
        public static void RemoveAll(IEnumerable<string> keys)
        {
            if (!IsCache) return;
            if (keys == null || !keys.Any())
            {
                return;
            }
            keys.ToList().ForEach(item => Cache.Remove(item));

        }
        #endregion

        #region 获取缓存

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public static T Get<T>(string key, T def)
        {
            return Get(key).To<T>(def);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            return Get(key).To<T>();
        }


        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return string.IsNullOrEmpty(key) ? null : Cache.Get(key);
        }


        /// <summary>
        /// 获取缓存集合
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        /// <returns></returns>
        public static IDictionary<string, object> GetAll(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            var dict = new Dictionary<string, object>();

            keys.ToList().ForEach(item => dict.Add(item, Cache.Get(item)));

            return dict;
        }

        #endregion

        #region 释放

        /// <summary>
        /// 释放
        /// </summary>
        /// <returns></returns>
        public static void Dispose()
        {
            if (Cache != null)
                Cache.Dispose();
            //GC.SuppressFinalize(this);
        }

        #endregion
    }
}
