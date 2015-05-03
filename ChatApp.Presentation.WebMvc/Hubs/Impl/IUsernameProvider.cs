using Microsoft.AspNet.SignalR.Hubs;

namespace ChatApp.Presentation.WebMvc.Hubs.Impl
{
    public interface IUsernameProvider
    {
        string GetUsername(HubCallerContext context);
    }
}