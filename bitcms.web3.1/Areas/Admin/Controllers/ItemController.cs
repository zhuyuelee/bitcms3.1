using System.Collections.Generic;
using System.Web.Mvc;
using bitcms.Common;
using bitcms.UI;

namespace bitcms.web.Areas.Admin
{
    public class ItemController : BaseAdminController
    {
        //
        // GET: /Admin/Item/
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
        /// 检查编码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult checkcode(string itemcode, int itemid = 0)
        {
            using (var manage = new Data.CMSManage())
            {
                var count = manage.checkItemCode(itemcode.ToLower(), itemid);
                return getResult(manage.Error, manage.Message, new { Count = count });
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
                if (!(id > 0 && manage.deleteItem(id)))
                {
                    manage.Error = Entity.Error.错误;
                    manage.Message = "参数错误";

                }

                return getResult(manage.Error, manage.Message);
            }
        }

        /// <summary>
        /// 加载管理目录
        /// </summary>
        /// <returns></returns>
        /// <summary>
        public JsonResult loadjson(int id, string channel)
        {
            using (var manage = new Data.CMSManage())
            {
                List<Entity.ItemInfo> list = manage.getItemList(channel, id);
                var resultList = new List<dynamic>();
                list.ForEach(i =>
                {
                    resultList.Add(new
                    {
                        id = i.ItemId,
                        text = i.ItemName,
                        url = ""
                    });
                });

                return getResult(0, "", resultList);
            }
        }


        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        /// <summary>
        public JsonResult loadmodel(int id)
        {
            using (var manage = new Data.CMSManage())
            {
                return getResult(0, "", manage.getItemInfo(id));
            }
        }

        //
        // Post: /update/
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult update(Entity.ItemInfo info,string shows)
        {
            using (var manage = new Data.CMSManage())
            {
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
                manage.updateItem(info);

                return getResult(manage.Error, manage.Message);
            }
        }
    }
}