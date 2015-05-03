using System.Web;
using System.Web.Http;

namespace ChatApp.Application.WebApi
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            CompositeRoot.Instance.ComposeApp();
        }

        public override void Dispose()
        {
            CompositeRoot.Instance.Dispose();
            base.Dispose();
        }
    }
}
