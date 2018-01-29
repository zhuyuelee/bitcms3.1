using System.Web.Mvc;

namespace bitcms.web.Areas.Admin
{
    public class RecycleController : bitcms.UI.BaseAdminController
    {
        // GET: /Admin/News/
        public ActionResult Index(string channel)
        {
            return View();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public JsonResult loadjson(Entity.PageInfo page,  int itemid = 0)
        {
            using (var manage = new Data.CMSManage())
            {
                var list = manage.getDetailList(page, null, itemid, 0, 0, -1, true);
                return this.getPagination(page.TotalCount, page.PageNumber, list);
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public JsonResult loadcontent(int detailid)
        {
            using (var manage = new Data.CMSManage())
            {
                var list = manage.getDetailContentList(detailid);
                return this.getResult(Entity.Error.请求成功, "", list);
            }
        }
        /// <summary>
        /// 还原
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult restore(string ids)
        {
            using (var manage = new Data.CMSManage())
            {
                manage.restoreDetails(ids);
                return this.getResult(manage.Error, manage.Message);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult delete(string ids)
        {
            using (var manage = new Data.CMSManage())
            {
                manage.recycleDetails(ids);
                return this.getResult(manage.Error, manage.Message);
            }
        }

    }
}