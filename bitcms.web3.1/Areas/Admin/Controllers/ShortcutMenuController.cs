using System.Collections.Generic;
using System.Web.Mvc;
using bitcms.Common;

namespace bitcms.web.Areas.Admin
{
    public class ShortcutMenuController : bitcms.UI.BaseAdminController
    {
        //
        // GET: /Admin/ShortcutMenu/
        public ActionResult Index()
        {
            using (var manage = new Data.CMSManage())
            {

                ViewBag.shortcut = manage.getsAdminMenuShortcutList(this.userOnlineInfo.AdminUserInfo.RoleId, this.userOnlineInfo.AdminUserInfo.UserId);
                var list = new List<Entity.AdminMenuInfo>();
                //超级管理员
                if (this.userOnlineInfo.AdminUserInfo.UserId == 1)
                {
                    list = manage.getAdminMenuList(1);
                }
                else
                {
                    list = manage.getPowerList(this.userOnlineInfo.AdminUserInfo.RoleId);
                }
                return View(list);
            }
        }

        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult update(string ids)
        {
            using (var manage = new Data.CMSManage())
            {
                //更新操作
                var list = new List<Entity.AdminMenuShortcutInfo>();
                var _ids = ids.Split(',');
                foreach (var id in _ids)
                {
                    var adminmenuid = Utils.strToInt(id);
                    if (adminmenuid > 0)
                    {
                        list.Add(new Entity.AdminMenuShortcutInfo()
                        {
                            AdminMenuId = adminmenuid,
                            RoleId = this.userOnlineInfo.AdminUserInfo.RoleId,
                            UserId = this.userOnlineInfo.AdminUserInfo.UserId
                        });
                    }
                }

                manage.updateAdminMenuShortcut(list, this.userOnlineInfo.AdminUserInfo.RoleId, this.userOnlineInfo.AdminUserInfo.UserId);
                return getResult(manage.Error, manage.Message);
            }
        }
    }
}