using System;
using System.Web.Mvc;
using bitcms.Common;
using bitcms.Entity;

namespace bitcms.UI
{
    public class BaseSiteController : BaseController
    {
        public BaseSiteController()
            : this(false) { }

        public BaseSiteController(bool safe)
            : base(safe)
        {
            this.setViewBag(this.config, this.userOnlineInfo);
        }
        /// <summary>
        /// OnActionExecuting 控制全站访问
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!this.validateUserPermission())
            {
                ViewBag.Title = "受限访问";
                ViewBag.Message = "抱歉, 您非法请求数据或IP访问受到限制, 您无法访问本站！";
                filterContext.Result = this.Content(this.getSystemTemplate("error.cshtml"));
                return;
            }
            var readConn = Fetch.getConnectionStrings(DataProvider.DbContext.readConn);
            var updateConn = Fetch.getConnectionStrings(DataProvider.DbContext.updateConn);

            if (string.IsNullOrEmpty(readConn) || string.IsNullOrEmpty(updateConn))
            {
                if (Fetch.fileExist(Fetch.getMapPath("/install/lock.ini")))
                {
                    ViewBag.Title = "数据库配置不正确";
                    ViewBag.Message = "抱歉, 数据库配置不正确, 无法访问！";
                    filterContext.Result = this.Content(this.getSystemTemplate("error.cshtml"));
                    return;
                }
                else
                {
                    filterContext.Result = new RedirectResult("/admin/install");
                    return;
                }
            }

            if (this.config.Closed)
            {
                object area = null;
                bool isManage = false;

                if (this.userOnlineInfo.AdminOnline)
                {
                    isManage = true;
                }
                else if (filterContext.RouteData.DataTokens.TryGetValue("area", out area))//管理操作
                {
                    using (var manage = new Data.CMSManage())
                    {
                        var keysInfo = manage.getDictionaryKeyInfo("area", area.ToString().ToLower());
                        if (keysInfo != null)
                        {
                            isManage = true;
                        }
                    }
                }

                if (!isManage)
                {
                    filterContext.Result = this.Content(this.getSystemTemplate("close.cshtml"));
                    return;
                }
            }

            base.OnActionExecuting(filterContext);
        }

       

        /// <summary>
        /// 校验用户是否可以访问
        /// </summary>
        /// <returns></returns>
        private bool validateUserPermission()
        {
            //非法请求
            if (this.userOnlineInfo.UnsafeVisitsNum >= 10)
            {
                return false;
            }
            // 如果IP访问列表有设置则进行判断
            if (!string.IsNullOrEmpty(config.IpDenyAccess) && config.IpDenyAccess.Trim() != "")
            {
                string[] regctrl = Utils.splitString(config.IpDenyAccess, "\n");
                if (!Utils.inIPArray(Fetch.getClientIP(), regctrl))
                {
                    return false;
                }
            }
            return true;
        }


    }
}
