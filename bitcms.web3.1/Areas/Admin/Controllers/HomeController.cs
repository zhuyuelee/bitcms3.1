using System.Collections.Generic;
using System.Web.Mvc;

namespace bitcms.web.Areas.Admin
{
    public class HomeController : bitcms.UI.BaseAdminController
    {
        //
        // GET: /Admin/Home/
        public ActionResult Index(string t)
        {
            if (t == "out")
            {
                Config.UserConfig.adminLogout();//管理员退出

                return RedirectToAction("index", "login");
            }
            else
            {
                //string url = string.Format("http://www.bitcms.net/cmsinfo.html?domain={0}&version={1}", this.config.SiteDomain.Replace("http://", ""), this.config.Version);
                //ViewBag.License = bitcms.Common.HttpHelper.HttpRequest(url, System.Text.Encoding.UTF8);

                ViewBag.menus = getPowerMenus(0, 0);
                return View();
            }
        }
        /// <summary>
        /// 加载会员菜单
        /// </summary>
        /// <param name="fatherid"></param>
        /// <param name="shortcat"></param>
        /// <returns></returns>
        public ActionResult main()
        {
            return View();
        }
        /// <summary>
        /// 加载会员菜单
        /// </summary>
        /// <param name="fatherid"></param>
        /// <param name="shortcat"></param>
        /// <returns></returns>
        public ActionResult load(int fatherid, int shortcat)
        {
            var menus = new List<Entity.AdminMenuInfo>();
            if (shortcat == 1)
            {
                menus.Add(new bitcms.Entity.AdminMenuInfo()
                {
                    Controller = "#",
                    MenuName = "快捷菜单",
                    AdminMenuId = 0,
                    Icon = "glyphicon glyphicon-paperclip"
                });
            }
            else
            {
                menus = getPowerMenus(fatherid, shortcat);
            }
            if (menus.Count > 0)
            {
                ViewBag.childMenus = getPowerMenus(menus[0].AdminMenuId, shortcat);
            }
            return PartialView(menus);
        }

        /// <summary>
        /// 获取会员菜单
        /// </summary>
        /// <param name="fatherid"></param>
        /// <param name="shortcat"></param>
        /// <returns></returns>
        public JsonResult loadjson(int fatherid, int shortcat)
        {
            List<Entity.AdminMenuInfo> list = getPowerMenus(fatherid, shortcat);

            return getResult(0, "", list);
        }

        /// <summary>
        /// 获取登陆用户的菜单
        /// </summary>
        /// <param name="fatherid"></param>
        /// <param name="shortcut"></param>
        /// <returns></returns>
        private List<Entity.AdminMenuInfo> getPowerMenus(int fatherid, int shortcut)
        {
            List<Entity.AdminMenuInfo> list = new List<Entity.AdminMenuInfo>();
            //主管理菜单
            using (var manage = new Data.CMSManage())
            {
                list = manage.getPowerList(fatherid, this.userOnlineInfo.AdminUserInfo.RoleId, this.userOnlineInfo.AdminUserInfo.UserId, shortcut);
            }

            return list;
        }
    }
}