using System.Data.Common;
using System.Data.Entity;

namespace ChatApp.Persistence.EF
{
    public class ChatRoomContext : DbContext
    {
        public ChatRoomContext(DbConnection connection)
            : base(connection, false)
        {
        }

        public DbSet<ChatMessageData> Messages { get; set; }
    }
}
