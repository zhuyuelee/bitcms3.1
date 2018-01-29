using System.Web.Mvc;

namespace bitcms.UI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ErrorAttribute());
        }
    }

    public class ErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            string errorCode = filterContext.HttpContext.Request.Url.ToString();
            string msg = filterContext.Exception.ToString();
            if (filterContext.Exception.InnerException != null)
            {
                msg += "\nInnerException.Message" + filterContext.Exception.InnerException.Message;
            }
            //错误记录
            Common.Logs.error(errorCode, msg);
#if REFLEASE


            var url = string.Empty;

            // 跳转到错误页
            if (filterContext.Exception == null)
            {
                url += "/error";
            }
            else //It's an Http Exception, Let's handle it.  
            {
                var httpException = new HttpException(null, filterContext.Exception);
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // Page not found.  
                        url += "/error/404";
                        break;
                    case 500:
                        // Server error.  
                        url += "/error/500";
                        break;
                    // Here you can handle Views to other error codes.  
                    // I choose a General error template    
                    default:
                        url += "/error/";
                        break;
                }
            }
            string errorCode = filterContext.HttpContext.Request.Url.ToString();
            //错误记录
            //设置为true阻止golbal里面的错误执行
            filterContext.ExceptionHandled = true;
            filterContext.Result = new RedirectResult(url);
#endif
        }
    }
}
