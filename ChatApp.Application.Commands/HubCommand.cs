using System;
using ChatApp.Application.Commands.Base;

namespace ChatApp.Application.Commands
{
    public abstract class HubCommand : ICommand
    {
        public readonly string ConnectionId;

        protected HubCommand(string connectionId)
        {
            if (connectionId == "")
                throw new ArgumentException("connectionId");

            ConnectionId = connectionId;
        }
    }

    public class HubCommand<TCommand, TResponse> : Command<TCommand, TResponse>
        where TCommand : HubCommand<TCommand, TResponse>
        where TResponse : new()
    {
        public readonly string ConnectionId;

        protected HubCommand(string connectionId)
        {
            if (connectionId == "")
                throw new ArgumentException("connectionId");

            ConnectionId = connectionId;
        }
    }
}
