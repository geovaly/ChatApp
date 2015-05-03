using System.Collections.Specialized;
using System.Web;
using System.Web.Routing;

namespace ChatApp.Presentation.WebMvc.Services.Impl
{
    public class QueryStringAuthentificationWithoutPassword : IAuthenticationWithoutPassword
    {
        public bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(GetUsername());
        }

        public string GetUsername()
        {
            var result = (string)RouteDataValues[QueryStrings.Username];

            return string.IsNullOrEmpty(result)
                ? QueryString[QueryStrings.Username]
                : result;
        }

        public void SignIn(string username)
        {
            RouteDataValues[QueryStrings.Username] = username;
        }

        public void SignOut()
        {
            RouteDataValues.Remove(QueryStrings.Username);
        }

        private static RouteValueDictionary RouteDataValues
        {
            get { return HttpContext.Current.Request.RequestContext.RouteData.Values; }
        }
        private static NameValueCollection QueryString
        {
            get { return HttpContext.Current.Request.QueryString; }
        }
    }
}