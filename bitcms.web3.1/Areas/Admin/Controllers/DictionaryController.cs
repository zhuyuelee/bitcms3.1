using System.Collections.Generic;
using System.Web.Mvc;
using bitcms.Common;
using bitcms.UI;

namespace bitcms.web.Areas.Admin
{
    public class DictionaryController : BaseAdminController
    {
        //
        // GET: /Admin/Dictionaries/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 加载管理目录
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
                    List<Entity.DictionaryInfo> list = manage.getDictionaryList();
                    list.ForEach(i =>
                    {
                        resultList.Add(new
                        {
                            id = i.DictionaryId,
                            text = i.Name,
                            url = "#dictionary"
                        });
                    });
                }
                else
                {
                    List<Entity.DictionaryKeyInfo> list = manage.getDictionaryKeyList(Utils.strToInt(id));
                    list.ForEach(i =>
                    {
                        resultList.Add(new
                        {
                            id = i.KeyId,
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
                if (type == "#dictionary")
                    return getResult(0, "", manage.getDictionaryInfo(Utils.strToInt(id)));
                else
                {
                    var key = manage.getDictionaryKeyInfo(Utils.strToInt(id));
                    Entity.DictionaryInfo dictionary = null;
                    if (key != null)
                        dictionary = manage.getDictionaryInfo(key.DictionaryCode);
                    return getResult(0, "", new
                    {
                        dictionary = dictionary,
                        dictionarykey = key
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
        public JsonResult update(Entity.DictionaryInfo info)
        {
            using (var manage = new Data.CMSManage())
            {
                manage.updateDictionary(info);
                return getResult(manage.Error, manage.Message);
            }
        }

        /// <summary>
        /// 更新字典Key
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult updatekey(Entity.DictionaryKeyInfo info)
        {
            using (var manage = new Data.CMSManage())
            {
                manage.updateDictionaryKey(info);
                return getResult(manage.Error, manage.Message);
            }
        }

        /// <summary>
        /// 检查字典编码
        /// </summary>
        /// <param name="DictionaryCode"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string checkcode(string DictionaryCode, int did)
        {
            using (var manage = new Data.CMSManage())
            {
                return manage.checkDictionaryCode(DictionaryCode.ToLower(), did).ToString().ToLower();
            }
        }

        /// <summary>
        /// 检查字典编码
        /// </summary>
        /// <param name="DictionaryCode"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string checkkey(string value, string code, int keyid)
        {
            using (var manage = new Data.CMSManage())
            {
                return manage.checkDictionaryKey(keyid, code, value).ToString().ToLower();
            }
        }

        /// <summary>
        /// 删除 字典或key
        /// </summary>
        /// <returns></returns>
        public JsonResult delete(string id, string type)
        {
            using (var manage = new Data.CMSManage())
            {
                if (type == "#dictionary")
                    manage.deleteDictionary(Utils.strToInt(id));
                else
                    manage.deleteDictionaryKey(Utils.strToInt(id));

                return getResult(manage.Error, manage.Message);
            }
        }
    }
}