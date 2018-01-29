using System;
using System.Web.Mvc;
using bitcms.Common;
using bitcms.Config;
using bitcms.Entity;

namespace bitcms.UI
{
    public class BaseController : Controller
    {
        #region 变量
        /// <summary>
        /// 配置文件
        /// </summary>
        protected SiteConfigInfo config;

        /// <summary>
        /// 在线用户
        /// </summary>
        protected bitcms.Entity.UserOnlineInfo userOnlineInfo;
        #endregion

        #region 构造函数
        public BaseController()
            : this(false) { }
        /// <summary>
        /// 安全访问
        /// </summary>
        /// <param name="safe"></param>
        public BaseController(bool safe)
        {
            //加载配置文件
            this.config = SiteConfig.load();
            this.userOnlineInfo = bitcms.Config.UserConfig.getUserOnlineInfo(safe);
        }

        /// <summary>
        /// ViewBag配置
        /// </summary>
        /// <param name="normalconfig"></param>
        /// <param name="useronline"></param>
        protected void setViewBag(NormalConfigInfo normalconfig, BasicUserOnlineInfo useronline)
        {
            ViewBag.config = new NormalConfigInfo()
            {
                Address = normalconfig.Address,
                Closed = normalconfig.Closed,
                ClosedMessage = normalconfig.ClosedMessage,
                Email = normalconfig.Email,
                EnabledDetailVerifykey = normalconfig.EnabledDetailVerifykey,
                EnabledRegisterVerifykey = normalconfig.EnabledRegisterVerifykey,
                EnabledReviewVerifykey = normalconfig.EnabledReviewVerifykey,
                Fax = normalconfig.Fax,
                Icp = normalconfig.Icp,
                Phone = normalconfig.Phone,
                SeoDescription = normalconfig.SeoDescription,
                SeoKeywords = normalconfig.SeoKeywords,
                SeoTitle = normalconfig.SeoTitle,
                SiteDomain = normalconfig.SiteDomain,
                SiteName = normalconfig.SiteName,
                SitePath = normalconfig.SitePath,
                StatisticsCode = normalconfig.StatisticsCode,
                TimeZone = normalconfig.TimeZone
            };
            ViewBag.userOnline = new BasicUserOnlineInfo()
            {
                IP = userOnlineInfo.IP,
                UnsafeVisitsNum = useronline.UnsafeVisitsNum,
                StartDate = userOnlineInfo.StartDate,
                Url = userOnlineInfo.Url,
                UrlReferrer = userOnlineInfo.UrlReferrer,
                UserId = userOnlineInfo.UserId,
                UserInfo = userOnlineInfo.UserInfo,
                UserName = userOnlineInfo.UserName,
                UserOnlineId = userOnlineInfo.UserOnlineId
            };
        }
        #endregion

        #region 获取参数
        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        protected string getQueryString(string strName)
        {
            return getQueryString(strName, false);
        }

        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        protected string getQueryString(string strName, string defStr)
        {
            if (this.Request.QueryString[strName] == null)
            {
                return defStr;
            }
            return getQueryString(strName, false);
        }

        /// <summary>
        /// 获得指定Url参数的值
        /// </summary> 
        /// <param name="strName">Url参数</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>Url参数的值</returns>
        protected string getQueryString(string strName, bool sqlSafeCheck)
        {
            if (this.Request.QueryString[strName] == null)
            {
                return "";
            }

            if (sqlSafeCheck)
            {
                return Utils.removeUnSafeString(this.Request.QueryString[strName]);
            }
            return this.Request.QueryString[strName];
        }

        /// <summary>
        /// 获得QeryString参数INT
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected int getQueryInt(string key, int defValue)
        {
            string val = this.Request.QueryString[key];
            if (string.IsNullOrEmpty(val)) { return defValue; }
            int.TryParse(val, out defValue);
            return defValue;
        }

        /// <summary>
        /// 获得QeryString参数INT
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected int getQueryInt(string key)
        {
            return getQueryInt(key, 0);
        }

        /// <summary>
        /// 获得Form参数 STRING
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string getFormString(string key)
        {
            return getFormString(key, string.Empty);
        }

        /// <summary>
        /// 获得Form参数 STRING
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string getFormString(string key, string defValue)
        {
            string val = this.Request.Form[key];
            if (null == val) return defValue;
            return val.Trim();
        }

        /// <summary>
        /// 获得Form参数INT
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        protected int getFormInt(string key, int defValue)
        {
            string val = this.Request.Form[key];
            if (null == val) { return defValue; }
            int.TryParse(val, out defValue);
            return defValue;
        }

        /// <summary>
        /// 获得Form参数INT
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        protected int getFormInt(string key)
        {
            return getFormInt(key, 0);
        }
        #endregion

        #region 共用方法
        /// <summary>
        /// 返回JSON数据
        /// </summary>
        /// <param name="error">错误状态</param>
        /// <param name="messgae">消息</param>
        /// <returns></returns>
        public JsonResult getResult(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 返回JSON数据
        /// </summary>
        /// <param name="error">错误状态</param>
        /// <param name="messgae">消息</param>
        /// <returns></returns>
        public JsonResult getResult(Entity.Error error, string messgae)
        {
            return Json(new
            {
                error = error,
                message = messgae
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 返回JSON数据
        /// </summary>
        /// <param name="error">错误状态</param>
        /// <param name="messgae">消息</param>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
        public JsonResult getResult(Entity.Error error, string messgae, object data)
        {

            return Json(new
            {
                error = error,
                message = messgae,
                data = data
            }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region 模板页面
        /// <summary>
        /// 获取模板页面
        /// </summary>
        /// <param name="tempate"></param>
        /// <returns></returns>
        protected string getTemplate()
        {

            string html = null;
            this.ViewBag.parms = this.RouteData.DataTokens;

            #region 模板
            string template = "index.cshtml";
            //模板
            object viewTemp = null;
            this.RouteData.Values.TryGetValue("view", out viewTemp);
            if (viewTemp != null)
            {
                template = viewTemp.ToString();
            }
            bool isWap = false;
            var templatePath = string.Format("~/views/{0}/", this.config.Home);
            if (Fetch.isMobile() && this.config.EnabledWap)
            {
                if (Fetch.fileExist(Fetch.getMapPath(templatePath + template.Replace(".cshtml", ".mobile.cshtml"))))
                {
                    template = template.Replace(".cshtml", ".mobile.cshtml");
                    isWap = true;
                }
            }
            templatePath += template;
            #endregion

            if (this.RouteData.Values.ContainsKey("cachetime"))
            {
                var cachetime = this.RouteData.Values["cachetime"];
                int cache = (int)cachetime;
                if (cache > 0)
                {
                    string cacheKey = Fetch.getRawUrl().ToLower();
                    html = TemplateCache.getCache(cacheKey, cache, isWap);
                    if (html == null)
                    {
                        html = getHTML(templatePath);
                        TemplateCache.setCache(cacheKey, html, cache, isWap);
                    }
                }
            }

            if (html == null)
            {
                html = getHTML(templatePath);
            }
            return html;
        }

        /// <summary>
        /// 获取系统模板
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        protected string getSystemTemplate(string template)
        {
            return getHTML(string.Format("~/views/shared/{0}", template));
        }
        /// <summary>
        /// 获取模板页面html代码
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string getHTML(string path)
        {
            IView view = ViewEngines.Engines.FindView(this.ControllerContext, path, string.Empty).View;
            if (view != null)
            {
                using (System.IO.StringWriter writer = new System.IO.StringWriter())
                {
                    ViewContext viewcontext = new ViewContext(this.ControllerContext, view, this.ViewData, this.TempData, writer);
                    viewcontext.View.Render(viewcontext, writer);
                    return writer.ToString();
                }
            }
            else
            {
                throw new Exception(string.Format("模板\"{0}\"不存在", path));
            }
        }
        #endregion


    }
}
