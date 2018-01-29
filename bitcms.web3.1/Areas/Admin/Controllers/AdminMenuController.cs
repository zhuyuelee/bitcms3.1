using System.Collections.Generic;
using System.Web.Mvc;

namespace bitcms.web.Areas.Admin
{
    public class AdminMenuController : bitcms.UI.BaseAdminController
    {

        //
        // GET: /menu/
        public ActionResult Index()
        {
            using (var manage = new Data.CMSManage())
            {
                //icon
                ViewBag.icon = manage.getDictionaryKeyList("bootstrapicon");

                //会员类型
                ViewBag.usertype = manage.getDictionaryKeyList("usertype", "1");
                //Area
                ViewBag.area = new SelectList(manage.getDictionaryKeyList("area"), "Value", "Value");
            }
            return View();
        }

        //
        // Post: /update/
        [HttpPost]
        public JsonResult update(Entity.AdminMenuInfo info)
        {
            using (var manage = new Data.CMSManage())
            {
                manage.updateAdminMenu(info);
                return getResult(manage.Error, manage.Message);
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
                if (id > 0)
                {
                    manage.deleteAdminMenu(id);
                }
                else
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
        public JsonResult loadjson(int id, string t)
        {
            List<Entity.AdminMenuInfo> list = getMenus(id, t);
            var resultList = new List<dynamic>();
            list.ForEach(i =>
            {
                //var icon = "";
                //if (!string.IsNullOrEmpty(i.Icon))
                //{
                //    icon = "<i class='" + i.Icon + "'></i>";
                //}
                resultList.Add(new
                {
                    id = i.AdminMenuId,
                    icon=i.Icon,
                    text = i.MenuName,
                    url = i.Controller
                });
            });

            return getResult(0, "", resultList);
        }

        /// <summary>
        /// 获取目录
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public JsonResult loadmenus(string t)
        {
            using (var manage = new Data.CMSManage())
            {
                //根节点
                List<Entity.AdminMenuInfo> list = getMenus(0, t);
                var selectList = new List<Entity.AdminMenuInfo>();
                selectList.Add(new Entity.AdminMenuInfo()
                {
                    FatherId = -1,
                    AdminMenuId = 0,
                    MenuName = "+根节点"
                });
                foreach (var root in list)
                {
                    root.MenuName = " " + root.MenuName;
                    selectList.Add(root);
                    var childList = getMenus(root.AdminMenuId, t);
                    childList.ForEach(g => g.MenuName = "├ " + g.MenuName);
                    selectList.AddRange(childList);
                }

                return getResult(0, "", selectList);
            }
        }

        /// <summary>
        /// 检查字典编码
        /// </summary>
        /// <param name="DictionaryCode"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string checkcode(string control, string area, int aid = 0, string parm = "")
        {
            using (var manage = new Data.CMSManage())
            {
                var exist = manage.checkController(control.ToLower(), area.ToLower(), aid, parm);
                return (!exist).ToString().ToLower();
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
                return getResult(0, "", manage.getAdminMenuInfo(id));
            }
        }
        /// <summary>
        /// 获取登陆用户的菜单
        /// </summary>
        /// <param name="fatherid"></param>
        /// <param name="shortcat"></param>
        /// <returns></returns>
        private List<Entity.AdminMenuInfo> getMenus(int fatherid, string type)
        {
            List<Entity.AdminMenuInfo> list = new List<Entity.AdminMenuInfo>();
            //主管理菜单
            using (var manage = new Data.CMSManage())
            {
                list = manage.getAdminMenuList(fatherid, type);
            }
            return list;
        }
	}
}