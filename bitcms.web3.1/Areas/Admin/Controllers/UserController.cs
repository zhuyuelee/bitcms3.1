using System.Web.Mvc;
using bitcms.Common;
using bitcms.Entity;

namespace bitcms.web.Areas.Admin
{
    public class UserController : bitcms.UI.BaseAdminController
    {
        //
        // GET: /Admin/Manager/
        public ActionResult Index()
        {
            using (var manage = new Data.CMSManage())
            {
                ViewBag.RoleType = manage.getDictionaryKeyList("usertype");
                ViewBag.Role = manage.getRoleEnabledList();
                return View(manage.getCityList(0));
            }
        }

        /// <summary>
        /// 提交数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult update(Entity.UserInfo info, string password)
        {
            using (var manage = new Data.CMSManage())
            {
                string shows = this.getFormString("shows");
                if (string.IsNullOrEmpty(shows))
                {
                    info.Show = 0;
                }
                else
                {
                    var show = 0;
                    if (shows.IndexOf(',') > 0)
                    {
                        var _shows = shows.Split(',');
                        foreach (var _s in _shows)
                        {
                            show += Utils.strToInt(_s);
                        }
                    }
                    else
                    {
                        show = Utils.strToInt(shows);
                    }

                    info.Show = show;
                }

                //更新操作
                manage.updateUser(info);
                if (!string.IsNullOrEmpty(password))
                {
                    manage.updatePassword(info.UserId, 0, password);
                }
                if (info.RoleId > 0)
                {
                    var roleInfo = manage.getRoleInfo(info.RoleId);
                    if (roleInfo.RoleType == "admin")//管理员
                    {
                        manage.updateAdminPassword(info.UserId);
                    }
                }
                return getResult(manage.Error, manage.Message);
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public JsonResult loadjson(PageInfo page)
        {
            using (var manage = new Data.CMSManage())
            {
                var list = manage.getUserList(page, null, verify: 0);
                return getPagination(page.TotalCount, page.PageNumber, list);
            }
        }

        /// <summary>
        /// 检查会员 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public JsonResult checkname(string field, int userid = 0)
        {
            using (var manage = new Data.CMSManage())
            {
                var count = manage.checkUserName(field, userid);
                return this.getResult(manage.Error, manage.Message, new { Count = count });
            }
        }

        /// <summary>
        /// 删除会员
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult delete(string ids)
        {
            using (var manage = new Data.CMSManage())
            {
                manage.deleteUsers(ids);
                return getResult(manage.Error, manage.Message);
            }
        }
    }
}