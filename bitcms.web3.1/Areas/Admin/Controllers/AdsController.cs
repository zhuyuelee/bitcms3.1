using System;
using System.Collections.Generic;
using System.Web.Mvc;
using bitcms.Common;

namespace bitcms.web.Areas.Admin
{
    public class AdsController : bitcms.UI.BaseAdminController
    {
        //
        // GET: /Admin/Ads/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 加载广告
        /// </summary>
        /// <returns></returns>
        /// <summary>
        public JsonResult loadjson(string id)
        {
            using (var manage = new Data.CMSManage())
            {
                var resultList = new List<dynamic>();
                if (string.IsNullOrEmpty(id))
                {
                    List<Entity.AdsInfo> list = manage.getAdsList();
                    list.ForEach(i =>
                    {
                        resultList.Add(new
                        {
                            id = i.AdsCode,
                            text = i.Title,
                            url = "#ads"
                        });
                    });
                }
                else
                {
                    List<Entity.AdsDetailInfo> list = manage.getAdsDetailList(0, id.ToLower(), null, false);
                    list.ForEach(i =>
                    {
                        resultList.Add(new
                        {
                            id = i.AdsDetailId,
                            text = i.Title,
                            url = "#key"
                        });
                    });
                }
                return getResult(0, "", resultList);
            }
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        /// <summary>
        public JsonResult loadmodel(string id, string type)
        {
            using (var manage = new Data.CMSManage())
            {
                if (type == "#ads")
                    return getResult(0, "", manage.getAdsInfo(id));
                else
                {
                    var adsDetailInfo = manage.getAdsDetailInfo(Common.Utils.strToInt(id));
                    Entity.AdsInfo adsInfo = null;
                    if (adsDetailInfo != null)
                        adsInfo = manage.getAdsInfo(adsDetailInfo.AdsCode);
                    return getResult(0, "", new
                    {
                        ads = adsInfo,
                        adsdetail = adsDetailInfo
                    });
                }
            }
        }

        /// <summary>
        /// 更新字典
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult update(Entity.AdsInfo info)
        {
            using (var manage = new Data.CMSManage())
            {
                info.InDate = DateTime.Now;
                var tags = this.getFormString("tags");
                info.Tags = Utils.trim(Utils.replace(tags, ",{2,}", ","), ",");

                manage.updateAds(info);
                return getResult(manage.Error, manage.Message);
            }
        }

        /// <summary>
        /// 更新广告详情
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult updateadsdetail(Entity.AdsDetailInfo info)
        {
            using (var manage = new Data.CMSManage())
            {
                manage.updateAdsDetail(info);
                return getResult(manage.Error, manage.Message);
            }
        }

        /// <summary>
        /// 检查字典编码
        /// </summary>
        /// <param name="DictionaryCode"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string checkcode(string adscode, int adsid)
        {
            using (var manage = new Data.CMSManage())
            {
                return (!manage.checkAdsCode(adscode.ToLower(), adsid)).ToString().ToLower();
            }
        }

        /// <summary>
        /// 删除广告
        /// </summary>
        /// <returns></returns>
        public JsonResult delete(string id, string type)
        {
            using (var manage = new Data.CMSManage())
            {
                if (type == "#ads")
                    manage.deleteAds(id);
                else
                    manage.deleteAdsDetails(Utils.strToInt(id));

                return getResult(manage.Error, manage.Message);
            }
        }
    }
}