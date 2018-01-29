using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace bitcms.web.Areas.Admin
{
    public class RoleController : bitcms.UI.BaseAdminController
    {
        //
        // GET: /Admin/Role/
        public ActionResult Index()
        {
            using (var manage = new Data.CMSManage())
            {
                var powerList = manage.getAdminMenuList(1);
                //会员类型
                ViewBag.usertype = manage.getDictionaryKeyList("usertype");
                return View(powerList);
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
                var list = manage.getRoleList();
                return getPagination(list.Count, 1, list);
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
                return getResult(0, "", manage.getRolePowerList(id));
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult update(Entity.RoleInfo info, string powerList)
        {
            using (var manage = new Data.CMSManage())
            {
                //更新操作
                JavaScriptSerializer jsHelper = new JavaScriptSerializer();
                var powers = new List<Entity.RolePowerInfo>();
                if (!string.IsNullOrEmpty(powerList))
                {
                    powers = jsHelper.Deserialize<List<Entity.RolePowerInfo>>(powerList);
                }
                manage.updateRoleInfo(info, powers);
                return getResult(manage.Error, manage.Message);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult delete(string ids)
        {
            using (var manage = new Data.CMSManage())
            {
                manage.deleteRoles(ids);
                return getResult(manage.Error, manage.Message);
            }
        }
    }
}