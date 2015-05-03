using System;
using System.Collections.Generic;
using System.Linq;
using ChatApp.Application.Queries;
using ChatApp.Application.Queries.Base;
using ChatApp.Application.Queries.ViewModels;

namespace ChatApp.Persistence.EF
{
    public class EfGetLastMessagesQuery : IListQueryHandler<ChatMessagesListQuery, ChatMessageViewModel>
    {
        private readonly Func<ChatRoomContext> _dbContextFactory;
        private readonly int _chatMessages;

        public EfGetLastMessagesQuery(int chatMessages, Func<ChatRoomContext> dbContextFactory)
        {
            _chatMessages = chatMessages;
            _dbContextFactory = dbContextFactory;
        }

        public List<ChatMessageViewModel> Handle(ChatMessagesListQuery query)
        {
            using (var db = _dbContextFactory.Invoke())
            {
                return db
                    .Messages
                    .OrderByDescending(m => m.Timestamp)
                    .Take(_chatMessages)
                    .OrderBy(m => m.Timestamp)
                    .Select(ToViewModel)
                    .ToList();
            }
        }

        private ChatMessageViewModel ToViewModel(ChatMessageData data)
        {
            return new ChatMessageViewModel()
            {
                Content = data.Content,
                Username = data.Username,
                Timestamp = data.Timestamp
            };
        }
    }
}
