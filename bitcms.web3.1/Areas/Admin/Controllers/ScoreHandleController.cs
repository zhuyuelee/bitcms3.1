using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bitcms.web.Areas.Admin
{
    public class ScoreHandleController : bitcms.UI.BaseAdminController
    {
        //
        // GET: /Admin/ScoreHandle/
        public ActionResult Index()
        {
            using (var manage = new Data.CMSManage())
            {
                var list = manage.getScoreEventList(1);
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
                var list = manage.getUserList(page, null);
                return getPagination(page.TotalCount, page.PageNumber, list);
            }
        }
        /// <summary>
        /// 获取会员绑定手机
        /// </summary>
        /// <returns></returns>
        public JsonResult loadmodel(int userid = 0)
        {
            using (var manage = new Data.CMSManage())
            {
                var userbindinfo = manage.getUserBindInfo("mobile", userid);
                return this.getResult(manage.Error, manage.Message, userbindinfo);
            }
        }

        //
        // Post: /update/
        [HttpPost]
        public JsonResult update(int userid, int score, string eventcode, string reason)
        {
            using (var manage = new Data.CMSManage())
            {
                manage.insertScoreLog(userid, score, eventcode, reason);
                return getResult(manage.Error, manage.Message);
            }
        }
	}
}