using System;

namespace ChatApp.Domain.Model
{
    public sealed class UserConnection : IEquatable<UserConnection>
    {
        public readonly string Id;

        public readonly DateTime Timespan;

        public UserConnection(string id, DateTime timespan)
        {
            Id = id;
            Timespan = timespan;
        }

        public override bool Equals(object obj)
        {
            return obj is UserConnection && Equals((UserConnection)obj);
        }

        public bool Equals(UserConnection obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return string.Equals(Id, obj.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Id + "-" + Timespan;
        }
    }
}
