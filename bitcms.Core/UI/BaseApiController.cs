using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.Mvc;
using bitcms.Common;
namespace bitcms.UI
{
    public class BaseApiController : BaseController
    {
        #region 构造函数
        public BaseApiController() : base(true) { }
        #endregion
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var signatureInfo = new Entity.SignatureInfo();

            if (TryUpdateModel(signatureInfo))//获取APPID
            {
                if (signatureInfo != null && !string.IsNullOrEmpty(signatureInfo.AppId) && !string.IsNullOrEmpty(signatureInfo.Nonce) && !string.IsNullOrEmpty(signatureInfo.Sign) && !string.IsNullOrEmpty(signatureInfo.Timestamp))
                {
                    if (!checkSign(signatureInfo))
                    {
                        filterContext.Result = getResult(Entity.Error.签名失败, "签名失败!");
                        return;
                    }
                }
                else
                {
                    filterContext.Result = getResult(Entity.Error.签名失败, "参数错误!");
                    return;
                }
            }
            else
            {
                filterContext.Result = getResult(Entity.Error.签名失败, "参数错误!");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
        //<summary>
        //签名
        //</summary>
        //<param name="signatureInfo"></param>
        //<returns></returns>
        private bool checkSign(Entity.SignatureInfo signInfo)
        {
            using (var manage = new Data.CMSManage())
            {
                var modelInfo = manage.getModuleInfo(signInfo.AppId.ToLower(), Entity.ModuleType.API);
                if (modelInfo != null && modelInfo.Enabled == 1)
                {
                    Int64 timestamp = 0;
                    Int64.TryParse(signInfo.Timestamp, out timestamp);
                    var span = Utils.getTimestamp() - timestamp;
                    if (modelInfo.TimestampExpired > 0 && (span > modelInfo.TimestampExpired || span < 0 - modelInfo.TimestampExpired))
                    {
                        return false;//时间戳过期
                    }
                    //获取参数
                    var sortDic = getRequest();
                    sortDic.Add("appsecret", modelInfo.AppSecret);
                    //MD5加密
                    var sign = getSign(sortDic);
                    return sign.Equals(signInfo.Sign);
                }
                else
                {
                    return false;
                }

            }
        }



        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <returns></returns>
        private SortedDictionary<string, string> getRequest()
        {
            SortedDictionary<string, string> sortDict = new SortedDictionary<string, string>();
            NameValueCollection coll;

            if (Fetch.isGet())
            {
                coll = this.Request.QueryString;
            }
            else
            {
                coll = this.Request.Form;
            }

            for (var i = 0; i < coll.Count; i++)
            {
                var key = coll.AllKeys[i];
                if (!string.IsNullOrEmpty(key) && key.ToLower() != "sign" && !string.IsNullOrEmpty(coll[key]))
                    sortDict.Add(key, coll[key]);
            }

            return sortDict;
        }

        /// <summary>
        /// 获取本地签名
        /// </summary>
        /// <returns></returns>
        private string getSign(SortedDictionary<string, string> dic)
        {
            StringBuilder sb_str = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dic)
            {
                if (sb_str.Length > 0)
                {
                    sb_str.Append("&");
                }
                sb_str.Append(temp.Key + "=" + temp.Value);
            }

            return Utils.MD5(sb_str.ToString());
        }

    }
}
