using System;
using System.Web;
using System.Web.Caching;

namespace bitcms.Common
{
    public class DataCache
    {
        #region 变量
        private static string appPrefix = "bitcms_";
        #endregion

        #region  获取缓存内容
        /// <summary>
        /// 获取缓存内容
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static object get(string Key)
        {
            return HttpRuntime.Cache[appPrefix + Key];
        }
        #endregion

        #region 设置缓存
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        public static void set(string Key, object value)
        {

            set(Key, value, null, DateTime.Now.AddSeconds(120), TimeSpan.Zero);

        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <param name="cacheDependency"></param>
        public static void set(string Key, object value, CacheDependency cacheDependency)
        {

            HttpRuntime.Cache.Insert(appPrefix + Key, value, cacheDependency);

        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <param name="cacheDependency"></param>
        /// <param name="dt"></param>
        public static void set(string Key, object value, CacheDependency cacheDependency, DateTime dt)
        {
            set(Key, value, cacheDependency, dt, TimeSpan.Zero);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <param name="cacheDependency"></param>
        /// <param name="ts"></param>
        public static void set(string Key, object value, CacheDependency cacheDependency, TimeSpan ts)
        {
            set(Key, value, cacheDependency, System.Web.Caching.Cache.NoAbsoluteExpiration, ts);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <param name="cacheDependency"></param>
        /// <param name="dt"></param>
        /// <param name="ts"></param>
        public static void set(string Key, object value, CacheDependency cacheDependency, DateTime dt, TimeSpan ts)
        {
            HttpRuntime.Cache.Insert(appPrefix + Key, value, cacheDependency, dt, System.Web.Caching.Cache.NoSlidingExpiration);
        }
        #endregion

        #region 清除缓存
        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="Key"></param>
        public static void remove(string Key)
        {
            if (HttpRuntime.Cache[appPrefix + Key] != null)
            {
                HttpRuntime.Cache.Remove(appPrefix + Key);
            }
        }
        #endregion
    }
}
