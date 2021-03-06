﻿using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;
using MyChy.Frame.Core.Common.Helper;
using MyChy.Frame.Core.Common.MemoryCache;

namespace MyChy.Frame.Core.Redis
{
    public class RedisServer
    {
        private const string Dateformat = "yyyy-MM-dd";

        private static readonly MemoryCacheService MemoryCache = null;

        private static readonly RedisConfig Config = null;

        public static bool IsCacheError = false;

        private static readonly Lazy<ConnectionMultiplexer> LazyConnection =
            new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(Config.Connect));

        private static ConnectionMultiplexer Redis => LazyConnection.Value;

        static RedisServer()
        {
            var config = new ConfigHelper();
            Config = config.Reader<RedisConfig>("config/Redis.json");
            MemoryCache = new MemoryCacheService(null);
            if (string.IsNullOrEmpty(Config?.Connect))
            {
                Config = new RedisConfig { IsCache = false };
            }
            else
            {
                if (Config.DefaultDatabase > 0)
                {
                    Config.Connect = Config.Connect + ",defaultDatabase=" + Config.DefaultDatabase;
                }
            }
            if (!Config.IsCache)
            {
                IsCacheError = true;
            }
            try
            {
                var connect = ConnectionMultiplexer.Connect(Config.Connect);
                var res = connect.ClientName;
            }
            catch (Exception exception)
            {
                Config.IsCache = false;
                LogHelper.Log(exception);
                IsCacheError = true;
            }
            //finally
            //{
            //    Config.IsCache = false;
            //    IsCacheError = true;
            //}

        }

        //private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        //{
        //    return ConnectionMultiplexer.Connect(constr);
        //});

        //public static ConnectionMultiplexer redis
        //{
        //    get
        //    {
        //        return lazyConnection.Value;
        //    }
        //}

        /// <summary>
        /// 获得缓存接口类
        /// </summary>
        /// <returns></returns>
        public static IDatabase GetDatabase()
        {
            try
            {
                return Redis.GetDatabase();
            }
            catch (Exception exception)
            {
                LogHelper.Log(exception);
                IsCacheError = true;
            }

            return null;
        }

        #region 删除缓存

        public static void Remove(string key)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            if (IsCacheError) return;
            redisdb.KeyDelete(Config.Name + key);
        }

        public static void RemoveAsync(string key)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            if (IsCacheError) return;
            redisdb.KeyDeleteAsync(Config.Name + key);

        }

        public static void RemoveDay(string key)
        {
            //Remove(key);
            key = key + DateTime.Now.Date.ToString("yyyy-MM-dd");
            Remove(key);
        }

        /// <summary>
        /// key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ExistsKey(string key)
        {
            if (!Config.IsCache || IsCacheError) return false;
            var redisdb = Redis.GetDatabase();
            if (IsCacheError) return false;
            return redisdb.KeyExists(Config.Name + key);
        }

        #endregion

        #region String缓存

        /// <summary>
        /// 获取String缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        public static T StringGetCache<T>(string key, T defVal)
        {
            if (!Config.IsCache || IsCacheError) return (T)defVal;
            var redisdb = Redis.GetDatabase();
            if (IsCacheError) return default(T);
            var obj = redisdb.StringGet(Config.Name + key);
            return SerializeHelper.StringToObj<T>(obj, defVal);
        }

        /// <summary>
        /// Hash列表 Name 值
        /// </summary>
        /// <param name="key"></param>
        public static T StringGetCache<T>(string key)
        {
            return StringGetCache<T>(key, default(T));
        }


        #region 同步增加 String缓存

        /// <summary>
        /// 添加缓存 10分钟
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="objObject">数据</param>
        public static void StringSetCache(string key, object objObject)
        {
            var time = DateTime.Now.AddSeconds(Config.CacheSeconds);
            StringSetCache(key, objObject, time);
        }

        /// <summary>
        /// 添加缓存 指定时间
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="objObject">数据</param>
        /// <param name="seconds">秒</param>
        public static void StringSetCache(string key, object objObject, int seconds)
        {
            var time = DateTime.Now.AddSeconds(seconds);
            StringSetCache(key, objObject, time);
        }

        /// <summary>
        /// 添加缓存 指定时间
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="objObject">数据</param>
        /// <param name="time"></param>
        public static void StringSetCache(string key, object objObject, DateTime time)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            var ts = DateTime.Now.Subtract(time).Duration();
            if (IsCacheError) return;
            redisdb.StringSet(Config.Name + key, obj, ts);
        }

        /// <summary>
        /// 添加缓存 指定时间
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="objObject">数据</param>
        public static void StringDaySetCache(string key, object objObject)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            var ts = DateTime.Now.Subtract(DateTime.Now.AddDays(1).Date).Duration();
            if (IsCacheError) return;
            redisdb.StringSet(Config.Name + key, obj, ts);
        }

        #endregion


        #region 异步增加 String缓存

        /// <summary>
        /// 添加缓存 10分钟
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="objObject">数据</param>
        public static void StringSetCacheAsync(string key, object objObject)
        {
            var time = DateTime.Now.AddSeconds(Config.CacheSeconds);
            StringSetCacheAsync(key, objObject, time);
        }

        /// <summary>
        /// 添加缓存 指定时间
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="objObject">数据</param>
        /// <param name="seconds">秒</param>
        public static void StringSetCacheAsync(string key, object objObject, double seconds)
        {
            var time = DateTime.Now.AddSeconds(seconds);
            StringSetCacheAsync(key, objObject, time);
        }

        /// <summary>
        /// 添加缓存 指定时间
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="objObject">数据</param>
        /// <param name="time"></param>
        public static void StringSetCacheAsync(string key, object objObject, DateTime time)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            var ts = DateTime.Now.Subtract(time).Duration();
            if (IsCacheError) return;
            redisdb.StringSetAsync(Config.Name + key, obj, ts);
        }

        /// <summary>
        /// 添加缓存 指定时间
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="objObject">数据</param>
        public static void StringDaySetCacheAsync(string key, object objObject)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            var ts = DateTime.Now.Subtract(DateTime.Now.AddDays(1).Date).Duration();
            if (IsCacheError) return;
            redisdb.StringSetAsync(Config.Name + key, obj, ts);
        }

        #endregion


        #endregion


        #region 原子计数器

        /// <summary>
        /// 原子加计数器 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cardinal"></param>
        /// <returns></returns>
        public static long Increment(string key, long cardinal)
        {
            if (!Config.IsCache || IsCacheError) return -1;
            var redisdb = Redis.GetDatabase();

            return IsCacheError ? -1 : redisdb.StringIncrement(Config.Name + key, cardinal);
        }

        /// <summary>
        /// 原子加计数器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long Increment(string key)
        {
            if (!Config.IsCache || IsCacheError) return -1;
            var redisdb = Redis.GetDatabase();
            return IsCacheError ? -1 : redisdb.StringIncrement(Config.Name + key);
        }

        /// <summary>
        /// 原子减计数器 第一次赋值后cardinal 管用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cardinal"></param>
        /// <returns></returns>
        public static long Decrement(string key, long cardinal)
        {
            if (!Config.IsCache || IsCacheError) return 0;
            var redisdb = Redis.GetDatabase();
            return IsCacheError ? 0 : redisdb.StringDecrement(Config.Name + key, cardinal);
        }

        /// <summary>
        /// 原子减计数器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long Decrement(string key)
        {
            if (!Config.IsCache || IsCacheError) return 0;
            var redisdb = Redis.GetDatabase();
            return IsCacheError ? 0 : redisdb.StringDecrement(Config.Name + key);
        }

        #endregion

        #region Set 无序存储数组

        ///// <summary>
        ///// Set列表增加数据
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="objObject">数据</param>
        //public static void SetGetCache(string key)
        //{
        //    if (!Config.IsCache || IsCacheError) return;
        //    var redisdb = GetDatabase();
        //    var obj = SerializeHelper.ObjToString(objObject);
        //    if (IsCacheError) return;
        //    redisdb.SetAdd(Config.Name + key, obj);
        //}

        /// <summary>
        /// Set列表增加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objObject">数据</param>
        public static void SetAddCache(string key, string objObject)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            if (IsCacheError) return;
            redisdb.SetAdd(Config.Name + key, obj);
        }

        /// <summary>
        /// Set列表增加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objObject">数据</param>
        public static void SetAddCacheAsync(string key, string objObject)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            if (IsCacheError) return;
            redisdb.SetAddAsync(Config.Name + key, obj);
        }

        /// <summary>
        /// 判断Set 列表是否存在数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objObject"></param>
        /// <returns></returns>
        public static bool SetContainsCache(string key, string objObject)
        {
            if (!Config.IsCache || IsCacheError) return false;
            var redisdb = GetDatabase();
            if (IsCacheError) return false;
            return redisdb.SetContains(Config.Name + key, objObject);
        }

        /// <summary>
        /// Set列表增加数据 按照 天存储
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objObject"></param>
        /// <param name="isDelYesterday">是否删除昨天列表</param>
        
        public static void SetDayAddCache(string key, string objObject,
            bool isDelYesterday = true)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            if (IsCacheError) return;
            if (isDelYesterday)
            {
                var keyday = key + DateTime.Now.Date.AddDays(-1).ToString(Dateformat);
                var val = ExistsKey(keyday + "Set");
                if (!val)
                {
                    StringDaySetCacheAsync(keyday + "Set", "1");
                    Remove(keyday);
                }
            }
            key = key + DateTime.Now.Date.ToString(Dateformat);

            SetAddCache(key, objObject);
        }

        /// <summary>
        /// Set列表增加数据 按照 天存储
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objObject"></param>
        /// <param name="isDelYesterday">是否删除昨天列表</param>
        
        public static void SetDayAddCacheAsync(string key, string objObject,
            bool isDelYesterday = true)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            if (IsCacheError) return;
            if (isDelYesterday)
            {
                var keyday = key + DateTime.Now.Date.AddDays(-1).ToString(Dateformat);
                var val = ExistsKey(keyday + "Set");
                if (!val)
                {
                    StringDaySetCacheAsync(keyday + "Set", "1");
                    Remove(keyday);
                }
            }
            key = key + DateTime.Now.Date.ToString(Dateformat);

            SetAddCacheAsync(key, objObject);
        }


        /// <summary>
        /// 判断Set 列表是否存在数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objObject"></param>
        /// <returns></returns>
        
        public static bool SetDayContainsCache(string key, string objObject)
        {
            if (!Config.IsCache || IsCacheError) return false;
            var redisdb = GetDatabase();
            if (IsCacheError) return false;
            key = key + DateTime.Now.Date.ToString(Dateformat);
            return redisdb.SetContains(Config.Name + key, objObject);
        }

        /// <summary>
        /// Hash列表 Name 删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        public static bool SetDelete(string key, string name)
        {
            if (!Config.IsCache || IsCacheError) return false;
            var redisdb = GetDatabase();
            if (IsCacheError) return false;
            return redisdb.SetRemove(Config.Name + key, name);
        }

        /// <summary>
        /// Hash列表 Name 删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        
        public static bool SetDayDelete(string key, string name
            )
        {
            if (!Config.IsCache || IsCacheError) return false;
            var redisdb = GetDatabase();
            if (IsCacheError) return false;
            key = key + DateTime.Now.Date.ToString(Dateformat);
            return redisdb.SetRemove(Config.Name + key, name);
        }

        #endregion

        #region List 有序存储数据

        ///// <summary>
        ///// Set列表增加数据
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="objObject">数据</param>
        //public static void ListAddCache(string key, string objObject)
        //{
        //    if (!Config.IsCache || IsCacheError) return;
        //    var redisdb = GetDatabase();
        //    var obj = SerializeHelper.ObjToString(objObject);
        //    if (IsCacheError) return;
        //    redisdb.ListInsertAfter(Config.Name + key, obj);


        #endregion

        #region hash 

        /// <summary>
        /// Hash列表增加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="objObject">数据</param>
        public static void HashAddCache(string key, string name, object objObject)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            if (IsCacheError) return;
            var hashlist = new HashEntry[] { new HashEntry(name, obj) };
            redisdb.HashSet(Config.Name + key, hashlist);
        }


        /// <summary>
        /// Hash列表原子增加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="cardinal">数据</param>
        public static long HashIncrementCache(string key, string name, long cardinal)
        {
            if (!Config.IsCache || IsCacheError) return -1;
            var redisdb = GetDatabase();
            if (IsCacheError) return -1;
            return redisdb.HashIncrement(Config.Name + key, name, cardinal);
        }

        /// <summary>
        /// Hash列表原子减少
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="cardinal">数据</param>
        public static long HashDecrementCache(string key, string name, long cardinal)
        {
            if (!Config.IsCache || IsCacheError) return 0;
            var redisdb = GetDatabase();
            return IsCacheError ? 0 : redisdb.HashDecrement(Config.Name + key, name, cardinal);
        }

        /// <summary>
        /// Hash列表增加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="objObject">数据</param>
        public static void HashAddCacheAsync(string key, string name, object objObject)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            if (IsCacheError) return;
            var hashlist = new HashEntry[] { new HashEntry(name, obj) };
            redisdb.HashSetAsync(Config.Name + key, hashlist);


        }


        ///// <summary>
        ///// Hash列表增加数据
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="list"></param>
        //public static void HashAddCacheAsync(string key,IDictionary<string,object> list)
        //{
        //    if (!Config.IsCache || IsCacheError) return;
        //    var redisdb = GetDatabase();
        //    var obj = SerializeHelper.ObjToString(objObject);
        //    if (IsCacheError) return;
        //    var hashlist = new HashEntry[] { new HashEntry(name, obj) };
        //    foreach (var i in list)
        //    {
        //        hashlist.
        //    }
        //    redisdb.HashSetAsync(Config.Name + key, hashlist);

        //}

        /// <summary>
        /// Hash列表 Name 值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        public static T HashGetCache<T>(string key, string name)
        {
            return HashGetCache<T>(key, name, default(T));
        }

        /// <summary>
        /// Hash列表 Name 值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="defVal"></param>
        public static T HashGetCache<T>(string key, string name, T defVal)
        {
            if (!Config.IsCache || IsCacheError) return (T)defVal;
            var redisdb = GetDatabase();
            if (IsCacheError) return (T)defVal;
            var obj = redisdb.HashGet(Config.Name + key, name);
            return SerializeHelper.StringToObj<T>(obj, defVal);
        }

        /// <summary>
        /// Hash列表 Name 值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        public static List<T> HashGetCache<T>(string key, IList<string> name)
        {
            if (!Config.IsCache || IsCacheError) return null;
            var redisdb = GetDatabase();
            if (IsCacheError) return null;
            IList<RedisValue> list = name.Select(i => (RedisValue)i).ToList();
            var obj = redisdb.HashGet(Config.Name + key, list.ToArray());
            return obj.Select(i => SerializeHelper.StringToObj<T>(i)).ToList();
        }

        /// <summary>
        /// Hash列表 Name 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        public static bool HashExistsCache(string key, string name)
        {
            if (!Config.IsCache || IsCacheError) return false;
            var redisdb = GetDatabase();
            if (IsCacheError) return false;
            return redisdb.HashExists(Config.Name + key, name);
        }

        /// <summary>
        /// Hash列表 Name 删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        public static bool HashDelete(string key, string name)
        {
            if (!Config.IsCache || IsCacheError) return false;
            var redisdb = GetDatabase();
            if (IsCacheError) return false;
            return redisdb.HashDelete(Config.Name + key, name);
        }

        #endregion

        #region Hash 天

        /// <summary>
        /// Hash列表 Name 值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        public static T HashDayGetCache<T>(string key, string name)
        {
            key = key + DateTime.Now.Date.ToString("yyyy-MM-dd");
            return HashGetCache<T>(key, name);
        }

        /// <summary>
        /// Hash列表 Name 值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="defVal"></param>
        public static T HashDayGetCache<T>(string key, string name, T defVal)
        {
            key = key + DateTime.Now.Date.ToString(Dateformat);
            return HashGetCache<T>(key, name, defVal);
        }


        /// <summary>
        /// Hash列表 Name 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        public static bool HashDayExistsCache(string key, string name)
        {
            key = key + DateTime.Now.Date.ToString(Dateformat);
            return HashExistsCache(key, name);
        }

        /// <summary>
        /// Hash列表 Name 删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        public static bool HashDayDelete(string key, string name)
        {
            key = key + DateTime.Now.Date.ToString(Dateformat);
            return HashDelete(key, name);
        }

        /// <summary>
        ///  Hash列表原子增加 按照 天存储
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="cardinal"></param>
        /// <param name="isDelYesterday">是否删除昨天列表</param>
        public static long HashIncrementDayCache(string key, string name, long cardinal,
            bool isDelYesterday = true)
        {
            if (!Config.IsCache || IsCacheError) return -1;
            HashDayDelYesterday(key, "HashIncrement", isDelYesterday);
            key = key + DateTime.Now.Date.ToString(Dateformat);
            return HashIncrementCache(key, name, cardinal);
        }

        /// <summary>
        ///  Hash列表原子减少 按照 天存储
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="cardinal"></param>
        /// <param name="isDelYesterday">是否删除昨天列表</param>
        public static long HashDecrementDayCache(string key, string name, long cardinal
            , bool isDelYesterday = true)
        {
            if (!Config.IsCache || IsCacheError) return 0;
            HashDayDelYesterday(key, "HashIncrement", isDelYesterday);
            key = key + DateTime.Now.Date.ToString(Dateformat);
            return HashDecrementCache(key, name, cardinal);
        }

        /// <summary>
        /// Hash列表增加数据 按照 天存储
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="objObject"></param>
        /// <param name="isDelYesterday">是否删除昨天列表</param>
        public static void HashDayAddCache(string key, string name, object objObject
            , bool isDelYesterday = true)
        {
            if (!Config.IsCache || IsCacheError) return;
            HashAddCache(key + DateTime.Now.Date.ToString(Dateformat), name, objObject);
            HashDayDelYesterday(key, "Hash", isDelYesterday);
        }


        /// <summary>
        /// Hash列表增加数据 按照 天存储
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="objObject"></param>
        /// <param name="isDelYesterday">是否删除昨天列表</param>
        public static void HashDayAddCacheAsync(string key, string name, object objObject
            , bool isDelYesterday = true)
        {
            if (!Config.IsCache || IsCacheError) return;
            HashAddCacheAsync(key + DateTime.Now.Date.ToString(Dateformat), name, objObject);
            HashDayDelYesterday(key, "Hash", isDelYesterday);
        }

        /// <summary>
        /// 删除上一天的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keyAttached"></param>
        /// <param name="isDelYesterday"></param>
        private static void HashDayDelYesterday(string key, string keyAttached, bool isDelYesterday = true)
        {
            if (!isDelYesterday) return;
            var keyday = key + DateTime.Now.Date.AddDays(-1).ToString(Dateformat);
            if (Config.IsWebCache && MemoryCache.IsCache)
            {
                var isdel = MemoryCache.Get(keyday, 0);
                if (isdel != 0) return;
                //var val = ExistsKey(keyday + keyAttached);
                //if (!val)
                //{
                // StringDaySetCacheAsync(keyday + keyAttached, "1");
                Remove(keyday);
                // }
                MemoryCache.Set(key, 1, 360);
            }
            else
            {
                var val = ExistsKey(keyday + keyAttached);
                if (val) return;
                StringDaySetCacheAsync(keyday + keyAttached, "1");
                Remove(keyday);
            }
        }

        #endregion

        #region zset

        /// <summary>
        /// Sorted列表增加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objObject">数据</param>
        /// <param name="score"></param>
        public static void SortedAddCache(string key, string objObject, int score)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            if (IsCacheError) return;
            redisdb.SortedSetAdd(Config.Name + key, obj, score);
        }

        /// <summary>
        /// Sorted列表移除数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objObject">数据</param>
        public static void SortedDeleteCache(string key, string objObject)
        {
            if (!Config.IsCache || IsCacheError) return;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            if (IsCacheError) return;
            redisdb.SortedSetRemove(Config.Name + key, obj);
        }


        /// <summary>
        /// Sorted列表原子增加排序数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objObject">数据</param>
        /// <param name="score"></param>
        public static double SortedSetIncrementCache(string key, string objObject, int score)
        {
            if (!Config.IsCache || IsCacheError) return 0;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            if (IsCacheError) return 0;
            return redisdb.SortedSetIncrement(Config.Name + key, obj, score);
        }

        /// <summary>
        ///  Sorted列表原子减排序数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objObject">数据</param>
        /// <param name="score"></param>
        public static double SortedSetDecrementCache(string key, string objObject, int score)
        {
            if (!Config.IsCache || IsCacheError) return 0;
            var redisdb = GetDatabase();
            var obj = SerializeHelper.ObjToString(objObject);
            if (IsCacheError) return 0;
            return redisdb.SortedSetIncrement(Config.Name + key, obj, score);
        }

        /// <summary>
        ///  Sorted列表原子减排序数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="statr">开始位置</param>
        /// <param name="stop">结束位置</param>
        public static IList<string> SortedSetRangeByRankCache(string key, long statr, long stop)
        {
            if (!Config.IsCache || IsCacheError) return null;
            var redisdb = GetDatabase();
            if (IsCacheError) return null;
            var list = redisdb.SortedSetRangeByRank(Config.Name + key, statr, stop);
            return list.ToStringArray();
        }

        /// <summary>
        ///  Sorted列表原子减排序数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="statr">开始位置</param>
        /// <param name="stop">结束位置</param>
        public static IList<string> SortedSetRangeByRankDescendingCache(string key, long statr, long stop)
        {
            if (!Config.IsCache || IsCacheError) return null;
            var redisdb = GetDatabase();
            if (IsCacheError) return null;
            var list = redisdb.SortedSetRangeByRank(Config.Name + key, statr, stop, Order.Descending);
            return list.ToStringArray();
        }

        #endregion

    }
}
