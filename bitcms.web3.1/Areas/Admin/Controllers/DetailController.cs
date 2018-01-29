using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using bitcms.Common;

namespace bitcms.web.Areas.Admin
{
    public class DetailController : bitcms.UI.BaseAdminController
    {

        // GET: /Admin/News/
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
                    var list = manage.getItemList(channel);
                    ViewBag.channel = channelInfo;
                    return View(list);
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
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public JsonResult loadjson(Entity.PageInfo page, string channel, int itemid = 0)
        {
            using (var manage = new Data.CMSManage())
            {
                var list = manage.getDetailList(page, channel, itemid, 0, -1, -1, false);
                return getPagination(page.TotalCount, page.PageNumber, list);
            }
        }
        /// <summary>
        /// 检查编码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult checkcode(string detailcode, int detailid)
        {
            using (var manage = new Data.CMSManage())
            {
                var count = manage.checkDetailCode(detailcode.ToLower(), detailid);
                return getResult(manage.Error, manage.Message, new { Count = count });
            }
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public JsonResult loadotherjson(int detailid)
        {
            using (var manage = new Data.CMSManage())
            {
                var gallerylist = manage.getDetailGalleryList(detailid);
                var content = manage.getDetailContentList(detailid);
                return this.getResult(Entity.Error.请求成功, "", new
                {
                    gallery = gallerylist,
                    content = content
                });
            }
        }
        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult update(Entity.DetailInfo info, string contents,string gallery)
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
                if (info.UserId <= 0 && this.userOnlineInfo.AdminOnline)
                {
                    info.UserId = this.userOnlineInfo.AdminUserInfo.UserId;
                }

                manage.updateDetail(info);

                //更新详情
                if (!string.IsNullOrEmpty(contents))
                {
                    JavaScriptSerializer jsHelper = new JavaScriptSerializer();
                    var contentlist = jsHelper.Deserialize<List<Entity.DetailContentInfo>>(contents);
                    if (contentlist != null)
                    {
                        manage.updateDetailContent(contentlist, info.DetailId, info.ItemId, info.ChannelCode);
                    }
                }

                if (!string.IsNullOrEmpty(gallery))
                {
                    //更新图库
                    JavaScriptSerializer jsHelper = new JavaScriptSerializer();
                    var gallerylist = jsHelper.Deserialize<List<Entity.DetailGalleryInfo>>(gallery);
                    if (gallerylist != null)
                    {
                        manage.updateDetailGallery(gallerylist, info.DetailId);
                    }
                }

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
                manage.deleteDetails(ids);
                return getResult(manage.Error, manage.Message);
            }
        }

    }
}