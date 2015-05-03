using System;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace ChatApp.Domain.Model.Tests
{
    public class ChatRoomTests
    {
        public static readonly DateTime Now = new DateTime(1988, 02, 01);
        public static readonly DateTime DummyDateTime = A.Dummy<DateTime>();


        [Fact]
        public void GivenAnEmptyChatRoom_WhenANewUserConnect_ThenTheUserShouldJoinChat()
        {
            var chat = new ChatRoom();

            ChatEvent chatEvent = chat.Connect("username", Now, DummyConnectionId());

            chatEvent.Should().Be(
                ChatEvent.UserJoinsChat("username", Now));
        }

        [Fact]
        public void GivenSomeConnectedUsers_WhenANewUserConnect_ThenTheUserShouldJoinChat()
        {
            var chat = new ChatRoom();
            chat.Connect("dummyUser1", DummyDateTime, DummyConnectionId());
            chat.Connect("dummyUser2", DummyDateTime, DummyConnectionId());

            ChatEvent chatEvent = chat.Connect("username", Now, DummyConnectionId());

            chatEvent.Should().Be(
                ChatEvent.UserJoinsChat("username", Now));
        }

        [Fact]
        public void GivenAConnectedUser_WhenTheUserDisconnect_ThenTheUserShouldLeaveChat()
        {
            var chat = new ChatRoom();
            chat.Connect("username", DummyDateTime, "connection");

            ChatEvent chatEvent = chat.Disconnect("connection", Now);

            chatEvent.Should().Be(
                ChatEvent.UserLeavesChat("username", Now));
        }

        [Fact]
        public void GivenAnEmptyChatRoom_WhenANewUserDisconnect_ThenNothingHappens()
        {
            var chat = new ChatRoom();

            ChatEvent chatEvent = chat.Disconnect("dummyConnection", Now);

            chatEvent.Should().Be(
                ChatEvent.NothingHappens(Now));
        }

        [Fact]
        public void GivenSomeConnectedUsers_WhenANotConnectedUserDisconnect_ThenNothingHappens()
        {
            var chat = new ChatRoom();
            chat.Connect("dummyUser1", DummyDateTime, DummyConnectionId());
            chat.Connect("dummyUser2", DummyDateTime, DummyConnectionId());

            ChatEvent chatEvent = chat.Disconnect("newConnection", Now);

            chatEvent.Should().Be(
               ChatEvent.NothingHappens(Now));
        }

        [Fact]
        public void GivenAConnectedUserWithOneConnection_WhenTheUserConnectWithANewConnection_ThenNothingHappens()
        {
            var chat = new ChatRoom();
            chat.Connect("username", DummyDateTime, DummyConnectionId());

            ChatEvent chatEvent = chat.Connect("username", Now, "newConnection");

            chatEvent.Should().Be(ChatEvent.NothingHappens(Now));
        }

        [Fact]
        public void GivenAConnectedUserWithManyConnections_WhenTheUserDisconnectFromOneConnection_ThenNothingHappens()
        {
            var chat = new ChatRoom();
            chat.Connect("username", DummyDateTime, "connection1");
            chat.Connect("username", DummyDateTime, "connection2");

            ChatEvent chatEvent = chat.Disconnect("connection1", Now);

            chatEvent.Should().Be(ChatEvent.NothingHappens(Now));
        }

        [Fact]
        public void GivenAConnectedUserWithManyConnections_WhenTheUserDisconnectFromAllConnections_ThenTheUserShouldLeaveChat()
        {
            var chat = new ChatRoom();
            chat.Connect("username", DummyDateTime, "connection1");
            chat.Connect("username", DummyDateTime, "connection2");

            chat.Disconnect("connection1", DummyDateTime);
            ChatEvent chatEvent = chat.Disconnect("connection2", Now);

            chatEvent.Should().Be(ChatEvent.UserLeavesChat("username", Now));
        }

        [Fact]
        public void GivenAConnectedUser_WhenTheUserConnectWithAnOldConnection_ThenNothingHappens()
        {
            var chat = new ChatRoom();
            chat.Connect("username", DummyDateTime, "connection");

            ChatEvent chatEvent = chat.Connect("username", Now, "connection");

            chatEvent.Should().Be(
                ChatEvent.NothingHappens(Now));
        }

        [Fact]
        public void CanGetConnectedUsers()
        {
            var chat = new ChatRoom();
            chat.Connect("username1", Now.AddMinutes(-1), "connection1");
            chat.Connect("username2", Now, "connection2");

            chat.ConnectedUsers.ShouldAllBeEquivalentTo(
                new[]
                {
                    new ConnectedUser("username1", new UserConnection("connection1", Now.AddMinutes(-1))),
                    new ConnectedUser("username2", new UserConnection("connection2", Now))
                });
        }

        [Fact]
        public void GivenAConnectedUserWithManyConnections_ThenTheJoinedAtDatetimeShouldBeTheTimestampOfTheFirstConnection()
        {
            var chat = new ChatRoom();
            chat.Connect("username", Now.AddMinutes(-2), DummyConnectionId());
            chat.Connect("username", Now.AddMinutes(-1), DummyConnectionId());

            chat.Connect("username", Now, DummyConnectionId());

            chat.ConnectedUserByUsername("username").Value
                .JoinedAt.Should().Be(Now.AddMinutes(-2));
        }

        [Fact]
        public void GivenUsersLimitReached_WhenANewUserConnect_ThenShouldThrowException()
        {
            var chat = new ChatRoom(2);
            chat.Connect("username1", DummyDateTime, DummyConnectionId());
            chat.Connect("username2", DummyDateTime, DummyConnectionId());

            Action act = () =>
                chat.Connect("username3", DummyDateTime, DummyConnectionId());

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void GivenUsersLimitReached_WhenAnExistingUserMakeANewConnection_ThenShouldNotThrow()
        {
            var chat = new ChatRoom(2);
            chat.Connect("username", DummyDateTime, "connection1");
            chat.Connect("other", DummyDateTime, DummyConnectionId());

            Action act = () =>
                chat.Connect("username", DummyDateTime, "connection2");

            act.ShouldNotThrow<InvalidOperationException>();
        }

        private static string DummyConnectionId()
        {
            return Guid.NewGuid().ToString();
        }

    }
}
