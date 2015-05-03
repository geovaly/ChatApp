using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using ChatApp.Application.Commands;
using ChatApp.Application.Commands.Responses;
using ChatApp.Application.Queries;
using ChatApp.Application.Queries.Base;
using ChatApp.Application.Queries.ViewModels;
using ChatApp.Domain.Model;

namespace ChatApp.Application.WebApi.Controllers
{
    public class ChatRoomController : ApiController
    {
        private readonly ChatRoom _chatRoom;
        private readonly IChatMessageRepository _messageRepository;
        private readonly IListQueryHandler<ChatMessagesListQuery, ChatMessageViewModel> _messagesQuery;

        public ChatRoomController(
            ChatRoom chatRoom,
            IChatMessageRepository messageRepository,
            IListQueryHandler<ChatMessagesListQuery, ChatMessageViewModel> messagesQuery)
        {
            _chatRoom = chatRoom;
            _messageRepository = messageRepository;
            _messagesQuery = messagesQuery;
        }

        public ConnectResponse Connect(ConnectCommand command)
        {
            lock (_chatRoom)
            {
                if (_chatRoom.UsersLimitReachedFor(command.UserName))
                    return FailedBecauseUserLimitReached();

                var response = Response<ConnectResponse>();
                var chatEvent = _chatRoom.Connect(command.UserName, response.Timestamp, command.ConnectionId);
                response.UserJoinsChatRoom = chatEvent.UserJoinsChat();
                return response;
            }
        }

        private ConnectResponse FailedBecauseUserLimitReached()
        {
            var response = Response<ConnectResponse>();
            response.FailedBecauseUserLimitReached = true;
            return response;
        }

        public DisconnectResponse Disconnect(DisconnectCommand command)
        {
            lock (_chatRoom)
            {
                var response = Response<DisconnectResponse>();
                var chatEvent = _chatRoom.Disconnect(command.ConnectionId, response.Timestamp);
                response.UserLeavesChatRoom = chatEvent.UserLeavesChat();
                return response;
            }
        }

        public SendMessageResponse SendMessage(SendMessageCommand command)
        {
            var username = Username(command.ConnectionId);
            var response = Response<SendMessageResponse>();

            _messageRepository.Save(new ChatMessage(
                username,
                command.Message,
                response.Timestamp));

            return response;
        }

        private string Username(string connectionId)
        {
            lock (_chatRoom)
            {
                var user = _chatRoom.ConnectedUserByConnection(connectionId);

                if (!user.HasValue)
                    throw new HttpResponseException(HttpStatusCode.Forbidden);

                return user.Value.Username;
            }
        }

        public List<ConnectedUserViewModel> GetConnectedUsers()
        {
            lock (_chatRoom)
            {
                return _chatRoom.ConnectedUsers.Select(u =>
                   new ConnectedUserViewModel()
                   {
                       JoinedAt = u.JoinedAt,
                       Username = u.Username
                   }).ToList();
            }
        }

        public List<ChatMessageViewModel> GetChatMessages()
        {
            return _messagesQuery.Handle(new ChatMessagesListQuery());
        }

        public BooleanResult GetUsersLimitReached(string username)
        {
            lock (_chatRoom)
            {
                return new BooleanResult()
                {
                    Value = _chatRoom.UsersLimitReachedFor(username)
                };
            }
        }

        private T Response<T>() where T : Response, new()
        {
            return new T()
            {
                Timestamp = DateTime.Now
            };
        }
    }
}
