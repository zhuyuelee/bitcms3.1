using System.Web.Mvc;
using bitcms.UI;

namespace bitcms.Web.Controllers
{
    public class MainController : BaseSiteController
    {

        //
        // GET: /main/
        public ActionResult Index()
        {
            return Content(this.getTemplate());
        }

       
    }
}