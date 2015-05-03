using System;

namespace ChatApp.Application.Queries.ViewModels
{
    public class ChatMessageViewModel
    {
        public string Username { get; set; }

        public string Content { get; set; }

        public DateTime Timestamp { get; set; }
    }
}