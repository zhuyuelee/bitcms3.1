using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace bitcms.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();//启用特性路由
            routes.Add(new UrlRoute());
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = Config.SiteConfig.load().Home, action = "index", id = UrlParameter.Optional },
                namespaces: Config.SiteConfig.getNameSpace()
            );
        }
    }

    public class UrlRoute : RouteBase
    {
        /// <summary>
        /// 处理路由信息
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public override RouteData GetRouteData(System.Web.HttpContextBase httpContext)
        {
            return checkRoute(httpContext);
        }
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return null;
        }

        /// <summary>
        /// 检查Url规则
        /// </summary>
        /// <param name="path"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        private RouteData checkRoute(System.Web.HttpContextBase httpContext)
        {
            var config = Config.SiteConfig.load();
            var path = httpContext.Request.AppRelativeCurrentExecutionFilePath;//获取相对路径 ~/xxx/xxx
            if (!path.EndsWith("/"))
            {
                path += "/";
            }

            string sitePath = config.SitePath;

            IList<Entity.BasicMapRouteInfo> list = Config.MapRouteConfig.load();
            if (list != null)
            {
                foreach (Entity.BasicMapRouteInfo rule in list)
                {
                    var lookFors = rule.LookFor.Split('\n');
                    foreach (var look in lookFors)
                    {
                        if (!string.IsNullOrEmpty(look))
                        {
                            string lookFor = string.Format("^{0}$", ResolveUrl(sitePath, look));
                            Regex re = new Regex(lookFor, RegexOptions.IgnoreCase);
                            //站内路径重写
                            if (re.IsMatch(path))
                            {
                                var data = new RouteData(this, new MvcRouteHandler());
                                data.Values.Add("controller", config.Home);
                                data.Values.Add("action", "Index");
                                data.Values.Add("view", rule.Template);
                                data.Values.Add("cachetime", rule.CacheTime);

                                if (!string.IsNullOrEmpty(rule.SendTo))
                                {
                                    var parms = re.Replace(path, rule.SendTo).Split('\n');
                                    if (parms.Length > 0)
                                    {
                                        Regex reparm = new Regex(@"^\$\d+$", RegexOptions.IgnoreCase);
                                        for (int i = 0; i < parms.Length; i++)
                                        {
                                            var parm = parms[i];
                                            var query = parm.Split('=');
                                            if (query.Length >= 2)
                                            {
                                                if (!data.DataTokens.ContainsKey(query[0]))
                                                {
                                                    if (query.Length >= 2)
                                                    {
                                                        if (reparm.IsMatch(query[1]))
                                                        {
                                                            if (query.Length > 2)
                                                                data.DataTokens.Add(query[0], query[2]);
                                                        }
                                                        else
                                                        {
                                                            data.DataTokens.Add(query[0], query[1]);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                //URL参数
                                if (httpContext.Request.QueryString != null && httpContext.Request.QueryString.Keys.Count > 0)
                                {
                                    foreach (string key in httpContext.Request.QueryString.Keys)
                                    {
                                        if (!data.DataTokens.ContainsKey(key))
                                            data.DataTokens.Add(key, httpContext.Request.QueryString[key]);
                                    }
                                }

                                return data;
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 处理路径
        /// </summary>
        /// <param name="sitePath"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ResolveUrl(string sitePath, string url)
        {
            string checkUrl;
            if (url.Length == 0)
                checkUrl = sitePath;
            else if (url.StartsWith("~/"))
                checkUrl = sitePath + url.Substring(2);

            else if (url.StartsWith("/"))
            {
                checkUrl = sitePath + url.Substring(1);
            }
            else
            {
                checkUrl = sitePath + url;
            }

            if (checkUrl.StartsWith("/"))
                checkUrl = "~" + checkUrl;
            else if (!checkUrl.StartsWith("~/"))
                checkUrl = "~/" + checkUrl;
            if (!checkUrl.EndsWith("/"))
                checkUrl = checkUrl + "/";

            return checkUrl;
        }
    }
}
