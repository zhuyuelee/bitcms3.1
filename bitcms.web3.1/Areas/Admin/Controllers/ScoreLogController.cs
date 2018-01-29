using System.Linq;
using System.Web.Mvc;

namespace bitcms.web.Areas.Admin
{
    public class ScoreLogController : bitcms.UI.BaseAdminController
    {
        //
        // GET: /Admin/ScoreLog/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public JsonResult loadjson(Entity.PageInfo page)
        {
            using (var manage = new Data.CMSManage())
            {
                var list = manage.getUserList(page, "");
                return getPagination(page.TotalCount, page.PageNumber, list);
            }
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public JsonResult loadscorelog(Entity.PageInfo page, int userid = 0)
        {
            using (var manage = new Data.CMSManage())
            {
                var list = manage.getScoreLogList(page, userid);
                return getPagination(page.TotalCount, page.PageNumber, list);
            }
        }

    }
}