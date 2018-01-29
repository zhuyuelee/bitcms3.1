using System.Collections.Generic;
using System.Web.Mvc;

namespace bitcms.web.Areas.Admin
{
    public class CityController : bitcms.UI.BaseAdminController
    {
        //
        // GET: /SocialHub/SocialHub/
        public ActionResult Index()
        {
            using (var manage = new Data.CMSManage())
            {
               
                var list = manage.getDictionaryKeyList("diliquhua");
                return View(list);
            }
        }


        /// <summary>
        /// 加载列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult loadcity()
        {
            using (var manage = new Data.CMSManage())
            {
                var citylist = new List<Entity.CityInfo>();
                var list = manage.getCityList(0);
                citylist.AddRange(list);
                list.ForEach(g =>
                {
                    citylist.AddRange(manage.getCityList(g.CityId));
                });

                return getResult(0, "获取成功", citylist);
            }
        }
        /// <summary>
        /// 加载列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult loadjson(int id)
        {
            using (var manage = new Data.CMSManage())
            {
                var list = manage.getCityList(id);

                var resultList = new List<dynamic>();
                list.ForEach(i =>
                {
                    resultList.Add(new
                    {
                        id = i.CityId,
                        text = i.CityName,
                        url = ""
                    });
                });

                return getResult(0, "获取成功", resultList);
            }
        }
        /// <summary>
        /// 加载实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult loadmodel(int id)
        {
            using (var manage = new Data.CMSManage())
            {
                var info = manage.getCityInfo(id);
                return getResult(0, "获取成功", info);
            }
        }
        /// <summary>
        /// 检查城市id
        /// </summary>
        /// <param name="DictionaryCode"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string checkcode(int cityid = 0)
        {
            using (var manage = new Data.CMSManage())
            {
                var cityinfo = manage.getCityInfo(cityid);
                return (cityinfo == null).ToString().ToLower();
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult update(Entity.CityInfo info)
        {
            using (var manage = new Data.CMSManage())
            {
                manage.updateCityInfo(info);
                return getResult(manage.Error, manage.Message);
            }
        }
       /// <summary>
        /// 删除
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult delete(int cityid = 0)
        {
            using (var manage = new Data.CMSManage())
            {
                manage.deleteCity(cityid);
                return getResult(manage.Error, manage.Message);
            }
        }

    }
}