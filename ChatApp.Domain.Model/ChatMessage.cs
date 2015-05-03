using System;

namespace ChatApp.Domain.Model
{
    public class ChatMessage
    {

        public ChatMessage(string username, string content, DateTime timestamp)
        {
            Content = content;
            Timestamp = timestamp;
            Username = username;
        }

        public string Username { get; private set; }

        public string Content { get; private set; }

        public DateTime Timestamp { get; private set; }

        public override string ToString()
        {
            return Username + ":" +
                   (Content.Length > 10
                    ? Content.Substring(0, 10) + "..."
                    : Content);
        }
    }
}
