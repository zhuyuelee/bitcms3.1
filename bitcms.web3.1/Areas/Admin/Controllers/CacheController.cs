using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using bitcms.Common;

namespace bitcms.web.Areas.Admin
{
    public class CacheController : bitcms.UI.BaseAdminController
    {
        //
        // GET: /Admin/Cache/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public JsonResult getCache(string key)
        {
            ///缓存文件夹
            var cachePath = this.config.SitePath + "cache/";
            if (!string.IsNullOrEmpty(key))
            {
                cachePath += Utils.replace(key, "__", "/") + "/";
            }
            string template = Fetch.getMapPath(cachePath);
            
            var cacheList = new List<Entity.CacheFileInfo>();

            if (Directory.Exists(template))
            {
                //目录
                var directionList = new DirectoryInfo(template).GetDirectories();
                if (directionList != null && directionList.Count() > 0)
                {
                    foreach (var dir in directionList)
                    {
                        cacheList.Add(new Entity.CacheFileInfo()
                        {
                            Cachekey = dir.Name,
                            CacheType = 0,
                            Size = ""
                        });
                    }
                }

                //缓存文件
                var fileList = new DirectoryInfo(template).GetFiles("*.cshtml");
                if (fileList != null && fileList.Count() > 0)
                {
                    foreach (var file in fileList)
                    {
                        cacheList.Add(new Entity.CacheFileInfo()
                        {
                            Cachekey = file.Name,
                            CacheType = 1,
                            Size = (file.Length / 1024).ToString("0.00K")
                        });
                    }
                }
                
            }

            if (string.IsNullOrEmpty(key))
            {
                //字典缓存
                var cachelist = TemplateCache.getCache();

                if (cachelist != null && cachelist.Count() > 0)
                {
                    foreach (var cache in cachelist)
                    {
                        cacheList.Add(new Entity.CacheFileInfo()
                        {
                            Cachekey = cache.Key,
                            CacheType = 2,
                            Size = (Encoding.Default.GetByteCount(cache.Value.CacheContent) / 1024).ToString("0.00K")
                        });
                    }
                }
            }
            return this.getResult(Entity.Error.请求成功, "请求成功", cacheList);
        }


        /// <summary>
        /// 删除缓存
        /// </summary>
        public JsonResult delete(string keys)
        {
            string template = this.config.SitePath + "cache/";
            if (!string.IsNullOrEmpty(keys))
            {
                string[] _keys = keys.Split(';');
                foreach (string _key in _keys)
                {
                    if (!string.IsNullOrEmpty(_key))
                    {
                        string[] cacheKey = _key.Split(':');
                        if (cacheKey.Length == 2)
                        {
                            try
                            {
                                var key = Utils.replace(cacheKey[0], "__", "/");
                                switch (cacheKey[1])
                                {
                                    case "0"://删除文件夹
                                        Directory.Delete(Fetch.getMapPath(template + key + "/"), true);
                                        break;
                                    case "1"://删除文件
                                        System.IO.File.Delete(Fetch.getMapPath(template + key));
                                        break;
                                    case "2"://删除缓存
                                        TemplateCache.deleteCache(key);
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
            }
            else
            {
                TemplateCache.clearCache();//清除所有
            }
            return this.getResult(Entity.Error.请求成功, "删除成功");
        }
	}
}