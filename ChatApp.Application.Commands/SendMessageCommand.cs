using System;
using ChatApp.Application.Commands.Responses;

namespace ChatApp.Application.Commands
{
    public class SendMessageCommand : HubCommand<SendMessageCommand, SendMessageResponse>
    {
        public readonly string Message;

        public SendMessageCommand(string connectionId, string message)
            : base(connectionId)
        {
            if (message == "")
                throw new ArgumentException("message");

            Message = message;
        }
    }
}