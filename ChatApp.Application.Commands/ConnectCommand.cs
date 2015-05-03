using System;
using ChatApp.Application.Commands.Responses;

namespace ChatApp.Application.Commands
{
    public class ConnectCommand : HubCommand<ConnectCommand, ConnectResponse>
    {
        public readonly string UserName;

        public ConnectCommand(string username, string connectionId)
            : base(connectionId)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("username");

            UserName = username;
        }
    }
}