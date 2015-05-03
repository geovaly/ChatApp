using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatApp.Domain.Model
{
    public sealed class ConnectedUser : IEquatable<ConnectedUser>
    {
        private readonly List<UserConnection> _connections;

        public ConnectedUser(string username)
            : this(username, Enumerable.Empty<UserConnection>())
        {
        }

        public ConnectedUser(string username, IEnumerable<UserConnection> connections)
        {
            Username = username;
            _connections = connections.ToList();
        }

        public ConnectedUser(string username, params UserConnection[] connections)
            : this(username, connections as IEnumerable<UserConnection>)
        {
        }

        public string Username { get; private set; }

        public DateTime JoinedAt
        {
            get { return _connections.Min(x => x.Timespan); }
        }

        internal void AddConnection(UserConnection connection)
        {
            _connections.Add(connection);
        }

        internal void RemoveConnection(string connectionId)
        {
            _connections.RemoveAll(c => c.Id == connectionId);
        }

        public IReadOnlyList<UserConnection> Connections
        {
            get { return _connections; }
        }

        public bool Equals(ConnectedUser other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Username, other.Username);
        }

        public override bool Equals(object obj)
        {
            return obj is ConnectedUser && Equals((ConnectedUser)obj);
        }

        public override string ToString()
        {
            return Username + "-" + JoinedAt;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Username.GetHashCode();
            }
        }

    }
}
