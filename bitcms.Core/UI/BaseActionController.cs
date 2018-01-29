using System.Web.Mvc;
using bitcms.Common;

namespace bitcms.UI
{
    public class BaseActionController : BaseSiteController
    {
        #region 构造函数
        public BaseActionController() { }
        public BaseActionController(bool safe)
            : base(safe) { }
        #endregion
        #region ActionExecuting
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            //post提交验证会员登录
            if (Fetch.isPost() && this.userOnlineInfo.UserId <= 0)
            {
                var checkPost = true;//检查是否是需要权限post
                if (!string.IsNullOrEmpty(this.config.VerifyPost))
                {
                    var verifyurls = string.Format("\n{0}\n", this.config.VerifyPost.ToLower());
                    var url = Fetch.getRawUrl().ToLower();
                    if (url.IndexOf('?') > -1)
                    {
                        url = Utils.subString(url, url.IndexOf('?'));
                    }
                    if (url.StartsWith(this.config.SitePath))
                    {
                        url = Utils.trimStart(url, this.config.SitePath);
                    }
                    if (url.EndsWith("/"))
                    {
                        url = url.TrimEnd('/');
                    }
                    var reg = string.Format("\n({0})?{1}/?\n", this.config.SitePath, url);
                    checkPost = !Utils.verifyMatch(reg, verifyurls);
                }
                if (checkPost)
                {
                    filterContext.Result = getResult(Entity.Error.登录超时, "登陆超时！");
                    return;
                }

            }
        #endregion
        }
    }
}