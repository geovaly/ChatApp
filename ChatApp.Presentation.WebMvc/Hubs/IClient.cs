using System.Collections.Generic;
using ChatApp.Application.Queries.ViewModels;

namespace ChatApp.Presentation.WebMvc.Hubs
{
    public interface IClient
    {
        string Username { get; }

        string ConnectionId { get; }

        void ConnectionFailedBecauseUserLimitReached();

        void ConnectionSucceed(IEnumerable<ConnectedUserViewModel> users, IEnumerable<ChatMessageViewModel> messages);
    }
}