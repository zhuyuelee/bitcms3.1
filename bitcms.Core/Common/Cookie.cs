using System;
using System.Web;

namespace bitcms.Common
{
    public class Cookie
    {
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        /// <param name="expires">过期时间(分钟)</param>
        public static void writeCookie(string name, string value, int expires = 0)
        {
            HttpCookie cookie = new HttpCookie(name);
           
            cookie.Value = Utils.urlEncode(value);
            if (expires > 0)
            {
                cookie.Expires = DateTime.Now.AddMinutes(expires);
            }

            string cookieDomain = Config.SiteConfig.getCookieDomain();
            if (!string.IsNullOrEmpty(cookieDomain) && HttpContext.Current.Request.Url.Host.IndexOf(cookieDomain.TrimStart('.')) > -1 && bitcms.Common.Utils.isDomain(HttpContext.Current.Request.Url.Host))
            {
                cookie.Domain = cookieDomain;
            }

            HttpContext.Current.Response.SetCookie(cookie);
        }


        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        /// <param name="expires">过期时间(分钟)</param>
        public static void writeCookie(string name, string key, string value, int expires = 0)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie == null)
            {
                cookie = new HttpCookie(name);
            }
            cookie[key] = Utils.urlEncode(value);

            if (expires > 0)
            {
                cookie.Expires = DateTime.Now.AddMinutes(expires);
            }

            string cookieDomain = Config.SiteConfig.getCookieDomain();
            if (!string.IsNullOrEmpty(cookieDomain) && HttpContext.Current.Request.Url.Host.IndexOf(cookieDomain.TrimStart('.')) > -1 && bitcms.Common.Utils.isDomain(HttpContext.Current.Request.Url.Host))
            {
                cookie.Domain = cookieDomain;
            }

            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>cookie值</returns>
        public static string getCookie(string name)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[name] != null)
                return Utils.urlDecode(HttpContext.Current.Request.Cookies[name].Value.ToString());

            return null;
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>cookie值</returns>
        public static string getCookie(string name, string key)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[name] != null && HttpContext.Current.Request.Cookies[name][key] != null)
                return Utils.urlDecode(HttpContext.Current.Request.Cookies[name][key].ToString());

            return null;
        }
    }
}
