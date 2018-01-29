using System.Web.Mvc;

namespace bitcms.web3._0.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "admin_default",
                "admin/{controller}/{action}/{id}",
                new { controller = "home", action = "index", id = UrlParameter.Optional },
                namespaces: new[] { "bitcms.web.Areas.Admin" }
            );
        }
    }
}