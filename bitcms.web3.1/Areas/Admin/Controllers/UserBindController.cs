using System.Linq;
using System.Web.Mvc;
using bitcms.UI;

namespace bitcms.web.Areas.Admin
{
    public class UserBindController : BaseAdminController
    {
        //
        // GET: /Admin/ScoreLog/
        public ActionResult Index()
        {
            using (var mange = new Data.CMSManage())
            {
                var list = mange.getModuleList(Entity.ModuleType.UserBind);
                return View(list);
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public JsonResult loadjson(Entity.PageInfo page)
        {
            using (var manage = new Data.CMSManage())
            {
                var list = manage.getUserList(page, null, verify: 0);
                return getPagination(page.TotalCount, page.PageNumber, list);
            }
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public JsonResult loaduserbind(Entity.PageInfo page, int userid = 0)
        {
            using (var manage = new Data.CMSManage())
            {
                var list = manage.getUserBindList(page, userid).ToList();
                return getPagination(page.TotalCount, page.PageNumber, list);
            }
        }

        /// <summary>
        /// 检查会员账号 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public JsonResult checkcode(string usercode)
        {
            using (var manage = new Data.CMSManage())
            {
                var userid = manage.checkUserBindCode(usercode);
                return this.getResult(manage.Error, manage.Message, new { UserId = userid });
            }
        }

        //
        // Post: /update/
        [HttpPost]
        public JsonResult update(Entity.UserBindInfo info)
        {
            using (var manage = new Data.CMSManage())
            {
                manage.updateUserBind(info);
                return getResult(manage.Error, manage.Message);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult delete(string ids)
        {
            using (var manage = new Data.CMSManage())
            {
                if (!string.IsNullOrEmpty(ids))
                {
                    manage.deleteUserBind(ids);
                }
                else
                {
                    manage.Error = Entity.Error.错误;
                    manage.Message = "参数错误";

                }

                return getResult(manage.Error, manage.Message);
            }
        }

    }
}