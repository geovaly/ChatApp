using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatApp.Application.Queries.ViewModels;
using Microsoft.AspNet.SignalR;

namespace ChatApp.Presentation.WebMvc.Hubs.Impl
{
    public class ChatHub : Hub, IChatRoom, IClient
    {
        private const string ConnectedUsersGroup = "ConnectedUsers";

        public static IUsernameProvider UsernameProvider;

        public static Func<IClient, IChatRoom, HubController> ControllerFactory;

        public void Connect()
        {
            MakeController().Connect();
        }

        public void Send(string message)
        {
            MakeController().Send(message);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            MakeController().Disconnect();
            return base.OnDisconnected(stopCalled);
        }

        public void Logout()
        {
            MakeController().Logout();
        }

        public void Typing(string message)
        {
            MakeController().Typing(message);
        }

        private HubController MakeController()
        {
            return ControllerFactory.Invoke(this, this);
        }

        void IChatRoom.AddConnection(string connectionId)
        {
            Groups.Add(connectionId, ConnectedUsersGroup);
        }

        void IChatRoom.RemoveConnection(string connectionId)
        {
            Groups.Remove(connectionId, ConnectedUsersGroup);
        }

        void IChatRoom.NotifyJoinRoom(string username, DateTime timestamp)
        {
            Clients.Group(ConnectedUsersGroup).joins(username, timestamp);
        }

        void IChatRoom.NotifyMessageReceived(string username, string message, DateTime timestamp)
        {
            Clients.Group(ConnectedUsersGroup).onMessageReceived(username, message, timestamp);
        }

        void IChatRoom.NotifyLeaveRoom(string username, DateTime timestamp)
        {
            Clients.Group(ConnectedUsersGroup).leaves(username, timestamp);
        }

        void IChatRoom.NotifyTyping(string username, string message)
        {
            Clients.Group(ConnectedUsersGroup).onTyping(username, message);
        }

        string IClient.Username
        {
            get { return UsernameProvider.GetUsername(Context); }
        }

        string IClient.ConnectionId
        {
            get { return Context.ConnectionId; }
        }

        void IClient.ConnectionFailedBecauseUserLimitReached()
        {
            Clients.Caller.connectionFailed("User limit reached");
        }

        void IClient.ConnectionSucceed(
            IEnumerable<ConnectedUserViewModel> users,
            IEnumerable<ChatMessageViewModel> messages)
        {
            Clients.Caller.connectionSucceed(users, messages);
        }
    }
}