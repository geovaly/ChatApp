using ChatApp.Application.Commands;
using ChatApp.Application.Commands.Base;
using ChatApp.Application.Commands.Responses;
using ChatApp.Application.Queries;
using ChatApp.Application.Queries.Base;

namespace ChatApp.Presentation.WebMvc.Hubs
{
    public class HubController
    {
        private readonly IClient _client;
        private readonly IChatRoom _room;
        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;

        public HubController(
            IClient client,
            IChatRoom room,
            ICommandBus commandBus,
            IQueryBus queryBus)
        {
            _client = client;
            _room = room;
            _commandBus = commandBus;
            _queryBus = queryBus;
        }

        public void Connect()
        {
            var response = new ConnectCommand(_client.Username, _client.ConnectionId)
                .Send(_commandBus);

            if (response.FailedBecauseUserLimitReached)
                _client.ConnectionFailedBecauseUserLimitReached();
            else
                ConnectionSucceed(response);
        }

        private void ConnectionSucceed(ConnectResponse response)
        {
            if (response.UserJoinsChatRoom)
                _room.NotifyJoinRoom(_client.Username, response.Timestamp);

            _room.AddConnection(_client.ConnectionId);
            _client.ConnectionSucceed(new ConnectedUsersListQuery().List(_queryBus),
                new ChatMessagesListQuery().List(_queryBus));
        }

        public void Send(string message)
        {
            var response = new SendMessageCommand(_client.ConnectionId, message)
                .Send(_commandBus);

            _room.NotifyMessageReceived(_client.Username, message, response.Timestamp);
        }

        public void Disconnect()
        {
            _room.RemoveConnection(_client.ConnectionId);

            var response = new DisconnectCommand(_client.ConnectionId)
                .Send(_commandBus);

            if (response.UserLeavesChatRoom)
                _room.NotifyLeaveRoom(_client.Username, response.Timestamp);
        }

        public void Logout()
        {
            Disconnect();
        }

        public void Typing(string message)
        {
            _room.NotifyTyping(_client.Username, message);
        }
    }
}