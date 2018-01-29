using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using bitcms.Common;

namespace bitcms.web.Areas.Admin
{
    public class MapRouteController : bitcms.UI.BaseAdminController
    {
        //
        // GET: /menu/
        public ActionResult Index()
        {
            //根节点
            List<Entity.MapRouteInfo> list = getChild(0);
            var selectList = new List<Entity.MapRouteInfo>();
            selectList.Add(new Entity.MapRouteInfo()
            {
                MapRouteId = 0,
                MapRouteName = "+根节点"
            });
            foreach (var root in list)
            {
                root.MapRouteName = " " + root.MapRouteName;
                selectList.Add(root);
            }
            ViewBag.root = new SelectList(selectList, "MapRouteId", "MapRouteName");


            string template = Fetch.getMapPath(string.Format("/views/{0}/", this.config.Home));
            //模板文件
            var templateList = new List<string>();
            templateList.Add("=请选择模板=");
            if (Directory.Exists(template))
            {
                FileInfo[] files = new DirectoryInfo(template).GetFiles("*.cshtml");
                foreach (FileInfo file in files)
                {
                    if (!file.Name.StartsWith("_"))
                    {
                        templateList.Add(file.Name);
                    }
                }
            }
            ViewBag.template = new SelectList(templateList);

            return View();
        }

        /// <summary>
        /// 更新路由信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult update(Entity.MapRouteInfo info)
        {
            using (var manage = new Data.MapRouteManage())
            {
                manage.update(info);
                return getResult(manage.Error, manage.Message);
            }
        }

        /// <summary>
        /// 更新路由
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult make()
        {
            using (var manage = new Data.MapRouteManage())
            {
                manage.updateBasicMapRouteConfig();
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
            using (var manage = new Data.MapRouteManage())
            {
                if (id > 0)
                {
                    manage.delete(id);
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
        public JsonResult loadjson(int id)
        {
            List<Entity.MapRouteInfo> list = getChild(id);
            var resultList = new List<dynamic>();
            list.ForEach(i =>
            {
                resultList.Add(new
                {
                    id = i.MapRouteId,
                    text = i.MapRouteName,
                });
            });

            return getResult(0, "", resultList);
        }


        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        /// <summary>
        public JsonResult loadmodel(int id)
        {
            using (var manage = new Data.MapRouteManage())
            {
                return getResult(0, "", manage.getInfo(id));
            }
        }
        /// <summary>
        /// 获取登陆用户的菜单
        /// </summary>
        /// <param name="fatherid"></param>
        /// <param name="shortcat"></param>
        /// <returns></returns>
        private List<Entity.MapRouteInfo> getChild(int fatherid)
        {
            List<Entity.MapRouteInfo> list = new List<Entity.MapRouteInfo>();
            //主管理菜单
            using (var manage = new Data.MapRouteManage())
            {
                list = manage.getList(fatherid);
            }
            return list;
        }
    }
}