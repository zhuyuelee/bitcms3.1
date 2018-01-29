using System.Web.Mvc;

namespace bitcms.web.Areas.Admin
{
    public class ReviewController : bitcms.UI.BaseAdminController
    {
        //
        // GET: /Admin/Review/
        public ActionResult Index(string channel)
        {
            using (var manage = new Data.CMSManage())
            {
                Entity.DetailChannelInfo channelInfo = null;
                if (!string.IsNullOrEmpty(channel))
                {
                    channelInfo = manage.getDetailChannelInfo(channel);

                }
                if (channelInfo != null)
                {
                    ViewBag.channel = channelInfo;
                    return View();
                }
                else
                {
                    ViewBag.Title = "该内容频道不存在";
                    ViewBag.Message = "缺少参数，该内容频道不存在";
                    return this.Content(this.getSystemTemplate("error.cshtml"));
                }
            }
        }

        /// <summary>
        /// 加载资讯
        /// </summary>
        /// <returns></returns>
        public JsonResult loadnews(Entity.PageInfo page,string channel, int itemid = 0)
        {
            using (var manage = new Data.CMSManage())
            {
                var list = manage.getDetailList(page, channel, itemid);

                return getPagination(page.TotalCount, page.PageNumber, list);
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public JsonResult loadjson(Entity.PageInfo page, string channel, int detailid = -1, int reviewid = 0, int verify = 0)
        {
            using (var manage = new Data.CMSManage())
            {
                var list = manage.getReviewList(page, channel, detailid, reviewid, 0, verify);
                return getPagination(page.TotalCount, page.PageNumber, list);
            }
        }

        /// <summary>
        /// 回复
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult reply(int id, string content)
        {
            using (var manage = new Data.CMSManage())
            {
                if (manage.addReply(id, content, this.userOnlineInfo.AdminUserInfo.UserId, 1))
                {
                    return this.getResult(manage.Error, manage.Message);
                }
                else
                {
                    return this.getResult(Entity.Error.错误, "更新出错");
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult delete(int id)
        {
            using (var manage = new Data.CMSManage())
            {
                if (manage.deleteReview(id))
                {
                    return this.getResult(manage.Error, manage.Message);
                }
                else
                {
                    return this.getResult(Entity.Error.错误, "操作出错");
                }
            }
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult verify(int id)
        {
            using (var manage = new Data.CMSManage())
            {
                if (manage.verifyReview(id))
                {
                    return this.getResult(manage.Error, manage.Message);
                }
                else
                {
                    return this.getResult(Entity.Error.错误, "操作出错");
                }
            }
        }


    }
}