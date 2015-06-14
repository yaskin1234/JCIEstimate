using IdentitySample.Models;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using StackExchange.Profiling;

namespace IdentitySample
{
    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=301868
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
            if (Request.IsLocal)
            {
                MiniProfiler.Start();
            }
        }

        protected void Application_EndRequest()
        {            
            MiniProfiler.Stop();            
        }
    } 
}


namespace System.Web.Mvc.Html {    

    public static class ActionLinkButtonHelper {       
        public static MvcHtmlString ActionLinkButton(this HtmlHelper htmlHelper, string buttonText, string actionName, string controllerName, RouteValueDictionary routeValues) {
            string href = UrlHelper.GenerateUrl("default", actionName, controllerName, routeValues, RouteTable.Routes, htmlHelper.ViewContext.RequestContext, false);
            string buttonHtml = string.Format("<input type=\"button\" title=\"{0}\" value=\"{0}\" onclick=\"location.href='{1}'\" class=\"button\" />",buttonText,href);
            return new MvcHtmlString(buttonHtml);
        }
    }
}
