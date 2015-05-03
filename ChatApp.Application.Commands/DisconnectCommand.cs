using ChatApp.Application.Commands.Responses;

namespace ChatApp.Application.Commands
{
    public class DisconnectCommand : HubCommand<DisconnectCommand, DisconnectResponse>
    {
        public DisconnectCommand(string connectionId)
            : base(connectionId)
        {
        }
    }
}