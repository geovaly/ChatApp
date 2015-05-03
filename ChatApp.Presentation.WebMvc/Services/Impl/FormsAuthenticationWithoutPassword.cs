using System.Web;
using System.Web.Security;

namespace ChatApp.Presentation.WebMvc.Services.Impl
{
    public class FormsAuthenticationWithoutPassword : IAuthenticationWithoutPassword
    {
        public bool IsAuthenticated()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }

        public string GetUsername()
        {
            return HttpContext.Current.User.Identity.Name;
        }

        public void SignIn(string username)
        {
            FormsAuthentication.SetAuthCookie(username, false);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }


    }
}