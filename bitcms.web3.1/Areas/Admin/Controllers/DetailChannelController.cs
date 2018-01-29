using System.Web.Mvc;

namespace bitcms.web.Areas.Admin
{
    public class DetailChannelController : bitcms.UI.BaseAdminController
    {
        //
        // GET: /Admin/Role/
        public ActionResult Index()
        {
            using (var manage = new Data.CMSManage())
            {
                //icon
                ViewBag.icon = manage.getDictionaryKeyList("bootstrapicon");
                var menulist = manage.getAdminMenuList(0, "admin");
                return View(menulist);
            }
        }

        /// <summary>
        /// 获取角色JSON数据
        /// </summary>
        /// <returns></returns>
        public JsonResult loadjson()
        {
            using (var manage = new Data.CMSManage())
            {
                var list = manage.getDetailChannelList();
                return getPagination(list.Count, 1, list);
            }
        }

        /// <summary>
        /// 检查编码
        /// </summary>
        /// <param name="DictionaryCode"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string checkcode(string channelcode, int type)
        {
            if (type == 1)
            {
                return true.ToString().ToLower();
            }
            else
            {
                using (var manage = new Data.CMSManage())
                {
                    return (!manage.checkDetailChannelCode(channelcode.ToLower())).ToString().ToLower();
                }
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult update(Entity.DetailChannelInfo info, string icon, int adminmenuid = 0)
        {
            using (var manage = new Data.CMSManage())
            {
                //更新操作
                manage.updateDetailChannel(info, adminmenuid, icon);
                return getResult(manage.Error, manage.Message);
            }
        }
    }
}