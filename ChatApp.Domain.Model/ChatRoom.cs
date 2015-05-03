using System;
using System.Collections.Generic;
using System.Linq;
using ChatApp.Utility;

namespace ChatApp.Domain.Model
{
    public class ChatRoom
    {
        private readonly List<ConnectedUser> _users;
        private readonly Dictionary<string, ConnectedUser> _usersByConnections;

        public ChatRoom(int usersLimit = Int32.MaxValue)
        {
            UsersLimit = usersLimit;
            _usersByConnections = new Dictionary<string, ConnectedUser>();
            _users = new List<ConnectedUser>();
        }

        public int UsersLimit { get; set; }

        public ChatEvent Connect(string username, DateTime now, string connectionId)
        {
            if (UsersLimitReachedFor(username))
                throw new InvalidOperationException("User limit reached");

            if (HasConnection(connectionId))
                return ChatEvent.NothingHappens(now);
     
            ConnectedUser user = FindUserOrDefaultWithNoConnections(username);
            ChatEvent chatEvent = JoinsChatIfUserHasNoConnection(now, user);
            AddConnection(now, connectionId, user);
            ModifyUsersToReflectEvent(user, chatEvent);
            return chatEvent;
        }

        public ChatEvent Disconnect(string connectionId, DateTime now)
        {
            if (!HasConnection(connectionId))
                return ChatEvent.NothingHappens(now);

            ConnectedUser user = _usersByConnections[connectionId];
            RemoveConnection(connectionId, user);
            ChatEvent chatEvent = LeavesChatIfUserHasNoConnection(now, user);
            ModifyUsersToReflectEvent(user, chatEvent);
            return chatEvent;
        }

        public bool UsersLimitReachedFor(string username)
        {
            return _users.Count >= UsersLimit
                   && !ConnectedUserByUsername(username).HasValue;
        }

        public IReadOnlyList<ConnectedUser> ConnectedUsers
        {
            get { return _users; }
        }

        public Maybe<ConnectedUser> ConnectedUserByUsername(string username)
        {
            return _users.FirstOrDefault(c => c.Username == username);
        }

        public Maybe<ConnectedUser> ConnectedUserByConnection(string connectionId)
        {
            ConnectedUser result;
            return _usersByConnections.TryGetValue(connectionId, out result)
                ? result : null;
        }

        public string UsernameOrDefaultFromConnection(string connectionId)
        {
            var user = ConnectedUserByConnection(connectionId);
            return user.HasValue ? user.Value.Username : "";
        }

        private bool HasConnection(string connectionId)
        {
            return _usersByConnections.ContainsKey(connectionId);
        }

        private void AddConnection(DateTime now, string connectionId, ConnectedUser user)
        {
            user.AddConnection(new UserConnection(connectionId, now));
            _usersByConnections.Add(connectionId, user);
        }

        private void RemoveConnection(string connectionId, ConnectedUser user)
        {
            _usersByConnections.Remove(connectionId);
            user.RemoveConnection(connectionId);
        }

        private void ModifyUsersToReflectEvent(ConnectedUser user, ChatEvent chatEvent)
        {
            if (chatEvent.UserJoinsChat())
                _users.Add(user);

            if (chatEvent.UserLeavesChat())
                _users.Remove(user);
        }

        private ConnectedUser FindUserOrDefaultWithNoConnections(string username)
        {
            return _users.FirstOrDefault(c => c.Username == username)
                   ?? new ConnectedUser(username);
        }

        private static ChatEvent JoinsChatIfUserHasNoConnection(DateTime now, ConnectedUser user)
        {
            return user.Connections.Any()
                ? ChatEvent.NothingHappens(now)
                : ChatEvent.UserJoinsChat(user.Username, now);
        }
        private static ChatEvent LeavesChatIfUserHasNoConnection(DateTime now, ConnectedUser user)
        {
            return user.Connections.Any()
                ? ChatEvent.NothingHappens(now)
                : ChatEvent.UserLeavesChat(user.Username, now);
        }

    }
}
