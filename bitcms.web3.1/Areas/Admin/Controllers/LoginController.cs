using System.Web.Mvc;

namespace bitcms.web.Areas.Admin
{
    public class LoginController : bitcms.UI.BaseSiteController
    {
        //
        // GET: /Admin/Login/
        public ActionResult Index()
        {
            return View();
        }

        //
        // POst: /Login/
        [HttpPost]
        public JsonResult check(string username, string password)
        {
            using (Data.CMSManage manage = new Data.CMSManage())
            {

                var accounttype = string.Empty;
                if (Common.Utils.isMobile(username))
                {
                    accounttype = "mobile";
                }
                else if (Common.Utils.isEmail(username))
                {
                    accounttype = "email";
                }
                Entity.UserInfo userInfo = null;
                if (!string.IsNullOrEmpty(accounttype))
                {
                    var userBindInfo = manage.getUserBindInfo(accounttype, username);
                    if (userBindInfo != null)
                    {
                        if (manage.checkUserPassword(userBindInfo.UserId, password, Entity.passwordType.manage))
                        {
                            userInfo = manage.getUserInfo(userBindInfo.UserId);
                        }
                    }
                }
                else
                {
                    userInfo = manage.checkLogin(username.ToLower(), password, Entity.passwordType.manage);
                }
                if (userInfo == null)
                {
                    manage.Error = Entity.Error.错误;
                    manage.Message = "验证失败！";
                }
                else
                {
                    if (userInfo.Deadline == null || (userInfo.Deadline != null && userInfo.Deadline.Value > Config.SiteConfig.getLocalTime()))
                    {
                        //设置管理员在线
                        Config.UserConfig.setAdminOnline(userInfo);
                    }
                    else
                    {
                        manage.Error = Entity.Error.错误;
                        manage.Message = "管理员角色已过期！";
                    }
                }
                return getResult(manage.Error, manage.Message);
            }
        }
    }
}