using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace bitcms.Common
{
    public class Fetch
    {
        #region appsetting配置
        /// <summary>
        /// 获取web.config的配置项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string getAppSettings(string key)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[key];
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 获取数据库配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string getConnectionStrings(string key)
        {
            try
            {
                if (System.Configuration.ConfigurationManager.ConnectionStrings[key] != null)
                {
                    return System.Configuration.ConfigurationManager.ConnectionStrings[key].ConnectionString;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 客户端信息
        /// <summary>
        /// 获取当前请求的原始 URL(URL 中域信息之后的部分,包括查询字符串(如果存在))
        /// </summary>
        /// <returns>原始 URL</returns>
        public static string getRawUrl()
        {
            return HttpContext.Current.Request.RawUrl.ToLower();
        }

        /// <summary>
        /// 获取当前请求 URL绝对路径
        /// </summary>
        /// <returns>URL绝对路径L</returns>
        public static string getUrl()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                int port = HttpContext.Current.Request.Url.Port;
                if (!(port == 80 || port == 443))
                    return string.Format("{0}://{1}:{2}{3}", HttpContext.Current.Request.Url.Scheme.ToLower(), HttpContext.Current.Request.Url.Host.ToLower(), port, Fetch.getRawUrl());
                else
                    return string.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme.ToLower(), HttpContext.Current.Request.Url.Host.ToLower(), Fetch.getRawUrl());
            }
            else
            {
                return "local";
            }
        }

        /// <summary>
        /// 获得当前客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string getClientIP()
        {
            string result = "";
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                if (string.IsNullOrEmpty(result))
                    result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(result))
                    result = HttpContext.Current.Request.UserHostAddress;
            }

            if (string.IsNullOrEmpty(result) || !Utils.isIP(result))
                result = "127.0.0.1";
            return result;
        }

        /// <summary>
        /// 是否是微信浏览
        /// </summary>
        /// <returns></returns>
        public static bool isWechat()
        {
            if (isMobile())
            {
                //HTTP_USER_AGENT
                string u = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];//MicroMessenger 
                Regex b = new Regex(@"MicroMessenger", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                return b.IsMatch(u);
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 是否是手机浏览
        /// </summary>
        public static bool isMobile()
        {
            HttpContext context = HttpContext.Current;
            HttpRequest request = context.Request;
            if (context != null && request != null)
            {
                if (!string.IsNullOrEmpty(request.ServerVariables["HTTP_VIA"]))
                {
                    return true;
                }
                string u = request.ServerVariables["HTTP_USER_AGENT"];
                Regex b = new Regex(@"android.+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|mobile|p(ixi|re)\/|plucker|pocket|psp|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino|ucweb|mqqbrowser|miuibrowser|pad|micromessenger", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                if (string.IsNullOrEmpty(u))
                {
                    return false;
                }

                if (b.IsMatch(u))
                {
                    return true;
                }
                return v.IsMatch(u.Substring(0, 4));
            }
            else
                return false;

        }
        #endregion

        #region IO处理
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string getMapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }

        /// <summary>
        /// 返回文件是否存在
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否存在</returns>
        public static bool fileExist(string filename)
        {
            return System.IO.File.Exists(filename);
        }

        /// <summary>
        /// 获取文件内容
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否存在</returns>
        public static string getFile(string filename)
        {
            return getFile(filename, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// 获取文件内容
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否存在</returns>
        public static string getFile(string filename, System.Text.Encoding encoding)
        {
            if (fileExist(filename))
            {
                string str;
                using (StreamReader sr = new StreamReader(filename, encoding))
                {
                    str = sr.ReadToEnd();
                    sr.Close();
                }
                return str;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool writeFile(string filePath, string str)
        {
            string path = filePath.Substring(0, filePath.LastIndexOf('\\'));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);//创建目录
            }

            StreamWriter fs = new StreamWriter(filePath, false, Encoding.UTF8, 200);
            try
            {
                fs.Write(str);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
            finally
            {
                fs.Close();
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        public static void delFile(string path)
        {
            if (fileExist(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch { }
            }
        }
        #endregion

        #region 客户端请求
        /// <summary>
        /// 判断当前页面是否接收到了Post请求
        /// </summary>
        /// <returns>是否接收到了Post请求</returns>
        public static bool isPost()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
                return HttpContext.Current.Request.HttpMethod.Equals("POST");
            else
                return false;
        }

        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        /// <returns>是否接收到了Get请求</returns>
        public static bool isGet()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
                return HttpContext.Current.Request.HttpMethod.Equals("GET");
            else
                return false;
        }

        #endregion
    }
}
