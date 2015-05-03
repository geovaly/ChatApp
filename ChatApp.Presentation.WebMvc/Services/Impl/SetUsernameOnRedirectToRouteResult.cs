using System.Web.Mvc;
using System.Web.Routing;

namespace ChatApp.Presentation.WebMvc.Services.Impl
{
    public class SetUsernameOnRedirectToRouteResult : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var result = filterContext.Result as RedirectToRouteResult;
            if (result == null) return;

            var username = RouteDataValues(filterContext)[QueryStrings.Username];
            if (username != null)
                result.RouteValues[QueryStrings.Username] = username;
        }

        private static RouteValueDictionary RouteDataValues(ResultExecutingContext filterContext)
        {
            return filterContext.HttpContext.Request.RequestContext.RouteData.Values;
        }
    }
}