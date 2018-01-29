using System.Web.Mvc;
using bitcms.UI;

namespace bitcms.web.Areas.Admin
{
    public class ChangePasswordController :BaseAdminController
    {
        //
        // GET: /Admin/ChangePassword/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="password"></param>
        /// <param name="newpassword"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult update(string password, string newpassword)
        {
            using (var manage = new Data.CMSManage())
            {
                manage.updatePassword(this.userOnlineInfo.AdminUserInfo.UserId, Entity.passwordType.manage, password, newpassword);
                return this.getResult(manage.Error, manage.Message);
            }
        }
    }
}