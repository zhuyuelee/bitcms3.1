using System.Web.Mvc;

namespace bitcms.web.Areas.Admin
{
    public class UploadConfigController : bitcms.UI.BaseAdminController
    {
        #region 配置
        //
        // GET: /Admin/Config/
        public ActionResult Index()
        {

            var uploadConfig = Config.UploadConfig.load();
            if (uploadConfig == null)
                uploadConfig = new Entity.UploadConfigInfo();
            using (var manage = new Data.CMSManage())
            {
                ViewBag.WatermarkFont = new SelectList(manage.getDictionaryKeyList("watermarkfont"), "Value", "Title", uploadConfig.WatermarkFont);
            }
            return View(uploadConfig);
        }

        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult update(Entity.UploadConfigInfo info)
        {
            var error = Entity.Error.请求成功;
            var message = string.Empty;
            info.AttachMaxSize = info.AttachMaxSize * 1024 * 1024;
            //更新操作
            if (!Config.UploadConfig.save(info))
            {
                error = Entity.Error.错误;
                message = "保存出错！";
            }
            return getResult(error, message);
        }
        #endregion
	}
}