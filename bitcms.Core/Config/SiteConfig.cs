using System;
using System.Text.RegularExpressions;
using bitcms.Common;
using bitcms.Entity;

namespace bitcms.Config
{
    public class SiteConfig
    {
        private static string configPath = Fetch.getMapPath(@"/config/site.config");
        private static object lockHelper = new object();
        private SiteConfig()
        {

        }
        /// <summary>
        /// 配置文件
        /// </summary>
        private static SiteConfigInfo config = null;
        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <returns></returns>
        public static SiteConfigInfo load()
        {
            if (config == null)
            {
                config = SerializationHelper.load(typeof(SiteConfigInfo), configPath) as SiteConfigInfo;
            }
            return config;
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool save(SiteConfigInfo info)
        {
            bool result = true;
            config = info;//配置文件
            lock (lockHelper)
            {
                try
                {
                    result = SerializationHelper.save(info, configPath);
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        #region 常用配置
        /// <summary>
        /// 网站目录
        /// </summary>
        /// <returns></returns>
        public static string getSitePath()
        {
            return load().SitePath;
        }
       
        /// <summary>
        /// 系统时间
        /// </summary>
        /// <returns></returns>
        public static DateTime getLocalTime()
        {
            int timeZone = load().TimeZone;
            return DateTime.UtcNow.AddMinutes(timeZone);
        }
        /// <summary>
        /// cookie域
        /// </summary>
        /// <returns></returns>
        public static string getCookieDomain()
        {
            return load().CookieDomain;
        }

        /// <summary>
        /// cookie域
        /// </summary>
        /// <returns></returns>
        public static string[] getNameSpace()
        {
            string _namespace = load().NameSpace;
            if (string.IsNullOrEmpty(_namespace))
            {
                _namespace = "bitcms.Web.Controllers";
            }
            _namespace = Regex.Replace(_namespace, "\r{1,}", "\n");
            _namespace = Regex.Replace(_namespace, "\n{2,}", "\n");
            return _namespace.Split('\n');
        }
        #endregion

        #region 配置
        /// <summary>
        /// 版本号
        /// </summary>
        public const string Version = "3.1.0";
        /// <summary>
        /// 版本名
        /// </summary>
        public const string VersionName = "RC mysql";
        /// <summary>
        /// 公司
        /// </summary>
        public const string AssemblyCompany = "kindo.cc今斗云";
        /// <summary>
        /// 产品名
        /// </summary>
        public const string AssemblyProduct = "bitcms.net";
        /// <summary>
        /// Copyright
        /// </summary>
        public const string AssemblyCopyright = "© bitcms.net";
        #endregion

    }

    public class TablePrefixAttribute : Attribute
    {
        public TablePrefixAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
