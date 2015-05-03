using System.Web;
using System.Web.Mvc;
using ChatApp.Presentation.WebMvc.Services.Impl;

namespace ChatApp.Presentation.WebMvc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            if (UsingQueryStringAuthentification())
                GlobalFilters.Filters.Add(new SetUsernameOnRedirectToRouteResult());
        }

        private static bool UsingQueryStringAuthentification()
        {
            return CompositeRoot.AuthentifactionWithoutPasswordType == typeof(QueryStringAuthentificationWithoutPassword);
        }
    }
}
