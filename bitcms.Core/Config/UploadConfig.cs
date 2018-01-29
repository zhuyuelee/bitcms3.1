using bitcms.Common;

namespace bitcms.Config
{
    public class UploadConfig
    {
        #region 构造函数
        private static string configPath = Fetch.getMapPath(@"/config/upload.config");
        private static object lockHelper = new object();
        private UploadConfig()
        {
        }
        #endregion

        /// <summary>
        /// 配置文件
        /// </summary>
        private static Entity.UploadConfigInfo config = null;
        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <returns></returns>
        public static Entity.UploadConfigInfo load()
        {
            if (config == null)
            {
                config = SerializationHelper.load(typeof(Entity.UploadConfigInfo), configPath) as Entity.UploadConfigInfo;
            }
            return config;
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool save(Entity.UploadConfigInfo info)
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
    }
}