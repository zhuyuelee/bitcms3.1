using System.Web.Mvc;

namespace bitcms.web.Areas.Admin
{
    public class ConfigController : bitcms.UI.BaseAdminController
    {
        #region 系统配置
        //
        // GET: /Admin/Config/
        public ActionResult Index()
        {
            using (var manage = new Data.CMSManage())
            {
                ViewBag.TimeZone = new SelectList(manage.getDictionaryKeyList("timezone"), "Value", "Title", this.config.TimeZone.ToString());
            }
            return View(this.config);
        }

        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult update(Entity.SiteConfigInfo info)
        {
            var error = Entity.Error.请求成功;
            var message = string.Empty;
            //更新操作
            if (Config.SiteConfig.save(info))
            {
                this.config = Config.SiteConfig.load();
            }
            else
            {
                error = Entity.Error.错误;
                message = "保存出错！";
            }
            return getResult(error, message);
        }
        #endregion
	}
}