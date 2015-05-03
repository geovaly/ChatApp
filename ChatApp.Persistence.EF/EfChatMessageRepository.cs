using System;
using ChatApp.Domain.Model;

namespace ChatApp.Persistence.EF
{
    public class EfChatMessageRepository : IChatMessageRepository
    {
        private readonly Func<ChatRoomContext> _dbContextFactory;

        public EfChatMessageRepository(Func<ChatRoomContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public void Save(ChatMessage message)
        {
            using (var db = _dbContextFactory.Invoke())
            {
                db.Messages.Add(ToData(message));
                db.SaveChanges();
            }
        }

        private ChatMessageData ToData(ChatMessage message)
        {
            return new ChatMessageData()
            {
                Content = message.Content,
                Username = message.Username,
                Timestamp = message.Timestamp
            };
        }

    }
}
