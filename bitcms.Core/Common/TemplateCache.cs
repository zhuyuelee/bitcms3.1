using System;
using System.Collections.Generic;
using System.Timers;
using bitcms.Entity;

namespace bitcms.Common
{
    public class TemplateCache
    {
        #region 变量
        private static string cachPath = Config.SiteConfig.getSitePath() + "cache";
        private static Dictionary<string, TempCacheInfo> dictCach = null;
        private static Timer timer = null;
        #endregion

        static TemplateCache()
        {
            if (dictCach == null)
            {
                dictCach = new Dictionary<string, TempCacheInfo>();
            }
            if (timer == null)
            {
                timer = new Timer(300000);//5分钟运行一次
                timer.AutoReset = true;
                timer.Enabled = true;
                timer.Elapsed += new ElapsedEventHandler(clearCache);
            }
        }

        /// <summary>
        /// 获取所有内存缓存
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, TempCacheInfo> getCache()
        {
            return dictCach;
        }

        /// <summary>
        /// 获取缓存key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string getKey(string key, bool isWap)
        {
            string parm = string.Empty;
            if (key.IndexOf('?') > -1)
            {
                parm = key.Substring(key.IndexOf('?') + 1);
                key = key.Substring(0, key.IndexOf('?'));
            }
            if (!key.EndsWith("/"))
            {
                key += "/";
            }
            key += "index.cshtml";
            if (!string.IsNullOrEmpty(parm))
            {
                key = key.Replace(".cshtml", "_parms_" + Utils.urlEncode(Utils.removeUnSafeString(parm)) + ".cshtml");
            }
            if (isWap)//手机和电脑缓存分开
            {
                return string.Format("{0}/mobile/{1}", cachPath, key);
            }
            else
            {
                return string.Format("{0}{1}", cachPath, key);
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string getCache(string key, int cacheTime, bool isWap)
        {
            key = getKey(key, isWap);
            if (cacheTime == -1)//文件缓存
            {
                return Fetch.getFile(Fetch.getMapPath(key));
            }
            else if (cacheTime >= 60)
            {
                string tempPath = Fetch.getMapPath(key);
                DateTime dt = System.IO.File.GetLastWriteTime(tempPath);
                if (dt.AddMinutes(cacheTime) > DateTime.Now)
                    return Fetch.getFile(tempPath);
                else
                    return null;
            }
            else if (cacheTime > 0)
            {
                if (dictCach.ContainsKey(key))
                {
                    TempCacheInfo cacheInfo = dictCach[key];
                    if (cacheInfo.LimitDate >= DateTime.Now)
                    {
                        return cacheInfo.CacheContent;
                    }
                    else
                    {
                        dictCach.Remove(key);
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <returns></returns>
        public static void setCache(string key, string content, double cacheTime, bool isWap)
        {
            if (cacheTime == 0)
            {
                return;
            }
            key = getKey(key, isWap);
            if (cacheTime == -1 || cacheTime >= 60)
            {
                string mapPath = Fetch.getMapPath(key);
                new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                {
                    Fetch.writeFile(mapPath, content);
                })).Start();

            }
            else if (cacheTime > 0)
            {
                TempCacheInfo cacheInfo = null;
                if (dictCach.ContainsKey(key))
                {
                    cacheInfo = dictCach[key];
                }
                else
                {
                    cacheInfo = new TempCacheInfo();
                }

                cacheInfo.LimitDate = DateTime.Now.AddMinutes(cacheTime);
                cacheInfo.CacheContent = content;

                dictCach[key] = cacheInfo;
            }
        }

        #region 删除缓存
        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static bool clearCache()
        {
            return clearCache(0);
        }

        /// <summary>
        /// 根据类型清除缓存 0全部 1内存 2文件缓存
        /// </summary>
        public static bool clearCache(int type)
        {
            try
            {
                if (type == 0)//全部
                {
                    dictCach = new Dictionary<string, TempCacheInfo>();
                    System.IO.Directory.Delete(Fetch.getMapPath(cachPath), true);
                }
                else if (type == 1)
                {
                    dictCach = new Dictionary<string, TempCacheInfo>();
                }
                else if (type == 2)
                {
                    System.IO.Directory.Delete(Fetch.getMapPath(cachPath), true);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 定时清除缓存委托方法
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private static void clearCache(object obj, ElapsedEventArgs e)
        {
            List<string> keys = new List<string>();
            foreach (string key in dictCach.Keys)
            {
                keys.Add(key);
            }

            foreach (string key in keys)
            {
                TempCacheInfo cacheInfo = dictCach[key];
                if (cacheInfo.LimitDate < DateTime.Now)
                {
                    dictCach.Remove(key);
                }
            }
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <returns></returns>
        public static void deleteCache(string key)
        {
            if (dictCach.ContainsKey(key))
            {
                dictCach.Remove(key);
            }
            else if (Fetch.fileExist(Fetch.getMapPath(key)))
            {
                Fetch.delFile(Fetch.getMapPath(key));
            }
        }
        #endregion
    }

    
}
