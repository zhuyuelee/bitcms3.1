using System.Web.Mvc;
using bitcms.Common;

namespace bitcms.UI
{
    public class BaseAdminController : BaseSiteController
    {
        public BaseAdminController()
        {
            if (this.userOnlineInfo.AdminOnline)
            {
                //管理员信息
                ViewBag.adminUserInfo = this.userOnlineInfo.AdminUserInfo;
            }
        }

        /// <summary>
        /// 控制后台访问
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (!this.validateAdminPermission())
            {
                ViewBag.Title = "受限访问";
                ViewBag.Message = "抱歉, 系统设置了IP访问列表限制, 您无法访问本网站！";
                filterContext.Result = this.Content(this.getSystemTemplate("error.cshtml"));
                return;
            }

            if (!this.userOnlineInfo.AdminOnline)
            {
                string url = Fetch.getRawUrl();
                if (!url.EndsWith("/"))
                {
                    url += "/";
                }
                if (url != "/admin/")
                {
                    filterContext.Result = getResult(Entity.Error.登录超时, "登陆超时！");
                }
                else
                {
                    filterContext.Result = new RedirectResult("/admin/login/");
                }
                return;
            }
            else if (this.userOnlineInfo.AdminUserInfo.UserId != 1)//权限控制
            {
                object area = null;
                object controller = null;
                if (filterContext.RouteData.Values.TryGetValue("controller", out controller) && filterContext.RouteData.DataTokens.TryGetValue("area", out area))
                {
                    if ( controller != null && area != null)
                    {
                        var _controller = controller.ToString().ToLower();
                        var _area = area.ToString().ToLower();
                        if (!(_area == "admin" && (_controller == "home" || _controller == "changepassword" || _controller == "shortcutmenu")))//排除管理主页和修改密码页
                        {
                            using (var manage = new bitcms.Data.CMSManage())
                            {
                                Entity.AdminMenuInfo meunInfo = null;
                                var menuList = manage.getAdminMenuList(_area, _controller);
                                if (menuList.Count == 1)
                                {
                                    meunInfo = menuList[0];
                                }
                                else if (menuList.Count > 1)
                                {
                                    //参数
                                    var url = Fetch.getRawUrl();
                                    if (url.IndexOf('?') > -1)
                                    {
                                        foreach (var info in menuList)
                                        {
                                            if (!string.IsNullOrEmpty(info.Parm) && url.IndexOf(info.Parm) > -1)
                                            {
                                                meunInfo = info;
                                                break;
                                            }
                                        }
                                    }
                                }
                                Entity.RolePowerInfo power = null;
                                if (meunInfo != null)
                                {
                                    power = manage.getRolePowerInfo(this.userOnlineInfo.AdminUserInfo.RoleId, meunInfo.AdminMenuId);
                                }

                                if (power == null)
                                {
                                    filterContext.Result = getResult(Entity.Error.无查看权限, "无查看权限!");
                                    return;
                                }
                                else if (Fetch.isPost() && power.Edit != 1)//post提交
                                {
                                    filterContext.Result = getResult(Entity.Error.无提交权限, "无提交权限!");
                                    return;
                                }
                                this.ViewBag.Power = power;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 校验管理员是否可以访问
        /// </summary>
        /// <returns></returns>
        private bool validateAdminPermission()
        {

            // 如果IP访问列表有设置则进行判断
            if (!string.IsNullOrEmpty(config.AdminIpAccess) && config.AdminIpAccess.Trim() != "")
            {
                string[] regctrl = Utils.splitString(config.IpDenyAccess, "\n");
                return Utils.inIPArray(Fetch.getClientIP(), regctrl);
            }
            return true;
        }


        /// <summary>
        /// 返回JSON分页数据
        /// </summary>
        /// <returns></returns>
        public JsonResult getPagination(int total, int pageindex, object list)
        {
            var json = new JsonResult();
            json.Data = new
            {
                total = total,
                pageindex = pageindex,
                rows = list
            };
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }
    }
}
