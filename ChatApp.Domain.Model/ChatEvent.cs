using System;

namespace ChatApp.Domain.Model
{
    public sealed class ChatEvent : IEquatable<ChatEvent>
    {

        public static readonly ChatEvent Null =
            new ChatEvent("", UserAction.NothingHappens, DateTime.MinValue);

        public readonly string Username;
        public readonly DateTime Timestamp;
        private readonly UserAction _action;

        private ChatEvent(string username, UserAction userAction, DateTime timestamp)
        {
            Username = username;
            _action = userAction;
            Timestamp = timestamp;
        }

        public static ChatEvent NothingHappens(DateTime timespan)
        {
            return new ChatEvent("", UserAction.NothingHappens, timespan);
        }

        public static ChatEvent UserJoinsChat(string username, DateTime timespan)
        {
            return new ChatEvent(username, UserAction.JoinsChat, timespan);
        }

        public static ChatEvent UserLeavesChat(string username, DateTime timespan)
        {
            return new ChatEvent(username, UserAction.LeavesChat, timespan);
        }

        public bool UserJoinsChat()
        {
            return _action == UserAction.JoinsChat;
        }

        public bool UserLeavesChat()
        {
            return _action == UserAction.LeavesChat;
        }

        public bool NothingHappens()
        {
            return _action == UserAction.NothingHappens;
        }

        private enum UserAction
        {
            NothingHappens, JoinsChat, LeavesChat
        }

        public bool Equals(ChatEvent other)
        {
            if (ReferenceEquals(null, other)) return false;

            return string.Equals(Username, other.Username)
                && Timestamp.Equals(other.Timestamp)
                && _action == other._action;
        }

        public override bool Equals(object obj)
        {
            return obj is ChatEvent && Equals((ChatEvent)obj);
        }


        public override string ToString()
        {
            return !Equals(Null)
                ? Username + "-" + _action + "-" + Timestamp
                : "Null";
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Username.GetHashCode();
                hashCode = (hashCode * 397) ^ Timestamp.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)_action;
                return hashCode;
            }
        }

        public static bool operator ==(ChatEvent a, ChatEvent b)
        {
            return ReferenceEquals(null, a)
                ? ReferenceEquals(null, b)
                : a.Equals(b);
        }

        public static bool operator !=(ChatEvent a, ChatEvent b)
        {
            return !(a == b);
        }

    }
}
