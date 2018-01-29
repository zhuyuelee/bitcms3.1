using System;
using System.IO;

namespace bitcms.Common
{
    public class Logs
    {

        private static Entity.SiteConfigInfo config { get { return bitcms.Config.SiteConfig.load(); } }
        //在网站根目录下创建日志目录
        private static string path = Fetch.getMapPath(config.SitePath + "app_data/log");

       
        /// <summary>
        /// 向日志文件写入调试信息
        /// </summary>
        public static void debug(string className, string content)
        {
            if (config.DebugLevel >= 3)
            {
                writeLog("DEBUG", className, content);
            }
        }
     

        /// <summary>
        /// 向日志文件写入运行时信息
        /// </summary>
        public static void info(string className, string content)
        {
            if (config.DebugLevel >= 2)
            {
                writeLog("INFO", className, content);
            }
        }


        /// <summary>
        /// 向日志文件写入出错信息
        /// </summary>
        public static void error(string className, string content)
        {
            if (config.DebugLevel >= 1)
            {
                writeLog("ERROR", className, content);
            }
        }

        /// <summary>
        /// 实际的写日志操作
        /// </summary>
        private static void writeLog(string type, string className, string content)
        {
            if (!Directory.Exists(path))//如果日志目录不存在就创建
            {
                Directory.CreateDirectory(path);
            }

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
            string filename = path + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//用日期对日志文件命名

            try
            {
                //创建或打开日志文件，向日志文件末尾追加记录ss
                string write_content = "\r\n" + type + "," + className + ": " + content + "\r\n" + time + " ";
                
                System.IO.File.AppendAllText(filename, write_content, System.Text.Encoding.UTF8);
            }
            catch
            {
            }

        }
    }
}
