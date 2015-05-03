using Microsoft.AspNet.SignalR.Hubs;

namespace ChatApp.Presentation.WebMvc.Hubs.Impl
{
    public class FromUserIdentity : IUsernameProvider
    {
        public string GetUsername(HubCallerContext context)
        {
            return context.User.Identity.Name;
        }
    }
}