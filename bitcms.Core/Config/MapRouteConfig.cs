using System.Collections.Generic;
using bitcms.Common;

namespace bitcms.Config
{
    public class MapRouteConfig
    {
        /// <summary>
        /// xml存放路径
        /// </summary>
        private static string configPath = Fetch.getMapPath(@"/config/maproute.config");
        /// <summary>
        /// 缓存Url规则
        /// </summary>
        private static List<Entity.BasicMapRouteInfo> routeList = null;
        private static object lockHelper = new object();

        /// <summary>
        /// 加载重写文件
        /// </summary>
        /// <returns></returns>
        public static List<Entity.BasicMapRouteInfo> load()
        {
            if (routeList == null)
            {
                routeList = SerializationHelper.load(typeof(List<Entity.BasicMapRouteInfo>), configPath, "Root") as List<Entity.BasicMapRouteInfo>;
            }
            return routeList;
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool save(List<Entity.BasicMapRouteInfo> list)
        {
            bool result = true;
            routeList = list;//配置文件
            lock (lockHelper)
            {
                try
                {
                    result = SerializationHelper.save(list, configPath, "Root");
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
