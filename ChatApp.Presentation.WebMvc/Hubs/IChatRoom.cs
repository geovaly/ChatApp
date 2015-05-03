using System;

namespace ChatApp.Presentation.WebMvc.Hubs
{
    public interface IChatRoom
    {
        void AddConnection(string connectionId);

        void RemoveConnection(string connectionId);

        void NotifyJoinRoom(string username, DateTime timestamp);

        void NotifyMessageReceived(string username, string message, DateTime timestamp);

        void NotifyLeaveRoom(string username, DateTime timestamp);

        void NotifyTyping(string username, string message);
    }
}