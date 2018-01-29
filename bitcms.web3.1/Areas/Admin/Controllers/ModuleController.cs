using System.Collections.Generic;
using System.Web.Mvc;
using bitcms.Common;

namespace bitcms.web.Areas.Admin
{
    public class ModuleController : bitcms.UI.BaseAdminController
    {
        //
        // GET: /Admin/Model/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取角色JSON数据
        /// </summary>
        /// <returns></returns>
        public JsonResult loadjson()
        {
            using (var manage = new Data.CMSManage())
            {
                return this.getPagination(0, 1, manage.getModuleList());
            }
        }

        /// <summary>
        /// 检查编码
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string checkcode(string modulecode, int moduleid = 0)
        {
            using (var manage = new Data.CMSManage())
            {
                return manage.checkModuleCode(modulecode.ToLower(), moduleid).ToString().ToLower();
            }
        }
   /// <summary>
        /// 检查编码
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string checkappid(string appid, int moduleid = 0)
        {
            using (var manage = new Data.CMSManage())
            {
                if (!string.IsNullOrEmpty(appid))
                    appid = appid.ToLower();
                return manage.checkModuleAppId(appid, moduleid).ToString().ToLower();
            }
        }
        /// <summary>
        /// 删除会员
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult delete(string ids)
        {
            using (var manage = new Data.CMSManage())
            {
                if (!string.IsNullOrEmpty(ids))
                {
                    string[] arrIds = ids.Split(',');
                    var delList = new List<Entity.ModuleInfo>();
                    foreach (string strId in arrIds)
                    {
                        var info = manage.getModuleInfo(strId);
                        if (info != null)
                        {
                            delList.Add(info);
                        }
                    }
                    manage.deleteModule(delList);
                }
                return getResult(manage.Error, manage.Message);
            }
        }

        /// <summary>
        /// 提交数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult update(Entity.ModuleInfo info)
        {
            using (var manage = new Data.CMSManage())
            {
                
                //更新操作
                manage.updateModule(info);

                return getResult(manage.Error, manage.Message);
            }
        }
    }
}