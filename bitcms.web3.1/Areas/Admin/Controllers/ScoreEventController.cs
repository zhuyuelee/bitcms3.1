using System.Web.Mvc;

namespace bitcms.web.Areas.Admin
{
    public class ScoreEventController : bitcms.UI.BaseAdminController
    {
        //
        // GET: /Admin/ScoreEvent/
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
                var list = manage.getScoreEventList(page, -1);
                return getPagination(page.TotalCount, page.PageNumber, list);
            }
        }

        /// <summary>
        /// 验证编码
        /// </summary>
        /// <param name="eventcode"></param>
        /// <param name="eventid"></param>
        /// <returns></returns>
        public string checkcode(string eventcode, int eventid = 0)
        {
            using (var manage = new Data.CMSManage())
            {
                return (!manage.checkScoreEventCode(eventid, eventcode.ToLower())).ToString().ToLower();
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult update(Entity.ScoreEventInfo info)
        {
            using (var manage = new Data.CMSManage())
            {
                info.EventCode = info.EventCode.ToLower();
                manage.updateScoreEvent(info);
                return this.getResult(manage.Error, manage.Message);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult delete(int id)
        {
            using (var manage = new Data.CMSManage())
            {
                manage.deleteScoreEvent(id);
                return this.getResult(manage.Error, manage.Message);
            }
        }
    }
}