using Microsoft.AspNet.SignalR.Hubs;

namespace ChatApp.Presentation.WebMvc.Hubs.Impl
{
    public class FromQueryString : IUsernameProvider
    {
        public string GetUsername(HubCallerContext context)
        {
            return context.QueryString[QueryStrings.Username];
        }
    }
}