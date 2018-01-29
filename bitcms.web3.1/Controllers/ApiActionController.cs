using System.Web.Mvc;

namespace bitcms.web.Controllers
{
    public class ApiActionController : UI.BaseApiController
    {
        //
        // GET: /ApiAction/
        public ActionResult Index()
        {
            return Content("api测试");
        }
    }
}