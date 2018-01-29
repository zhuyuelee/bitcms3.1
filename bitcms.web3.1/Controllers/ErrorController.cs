using System.Web.Mvc;
using bitcms.Common;
using bitcms.UI;

namespace bitcms.Web.Controllers
{
    public class ErrorController : bitcms.UI.BaseController
    {
        [Route("error")]
        [Route("error/{code}")]
        public ActionResult Index(int code = 0)
        {
            string title = "HTTP 发生错误";
            switch (code)
            { 
                case 404:
                    title = "无法找到文件";
                    break;
                case 500:
                    title = "内部服务器错误";
                    break;
            }
            if (code > 0)
            {
                ViewBag.ErrorCode = "_" + code;
                //错误记录
                Common.Logs.error(Fetch.getUrl(), "错误代码：" + code);
            }
            ViewBag.Title = title;

            return Content(this.getSystemTemplate("error.cshtml"));
        }
	}
}