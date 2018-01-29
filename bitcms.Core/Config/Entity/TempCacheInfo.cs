using System;

namespace bitcms.Entity
{
    /// <summary>
    /// 缓存实体
    /// </summary>
    public class TempCacheInfo
    {
        /// <summary>
        /// 缓存内容
        /// </summary>
        public string CacheContent { get; set; }
        /// <summary>
        /// 缓存过期日期
        /// </summary>
        public DateTime LimitDate { get; set; }
    }
    /// <summary>
    /// 缓存文件
    /// </summary>
    public class CacheFileInfo
    {
        /// <summary>
        /// 类型 0文件夹 1文件 2内存
        /// </summary>
        public int CacheType { get; set; }
        /// <summary>
        /// 缓存Key
        /// </summary>
        public string  Cachekey { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public string Size { get; set; }
    }
}
