var ChatRoom = {};

ChatRoom.Throttle_Send_Message = "400";
ChatRoom.Throttle_Remove_Typing = "4000";

ChatRoom.ChatMessage = function (sender, content, timestamp) {
    var self = this;
    self.username = sender;
    self.content = content;
    self.timestamp = new Date(timestamp);
}

ChatRoom.ConnectedUser = function (username, joinedAt) {
    var self = this;
    self.username = username;
    self.joinedAt = new Date(joinedAt);
    self.typing = ko.observable("");

    self.isTyping = ko.computed(function () {
        return self.typing() !== "";
    });

    self.onTyping = function (message) {
        self.typing(message);
    }

    self.throttledRemoveTyping = ko.computed(self.typing)
                                    .extend({ throttle: ChatRoom.Throttle_Remove_Typing });

    self.throttledRemoveTyping.subscribe(function () {
        if (self.typing() !== "")
            self.typing("");
    });
}

ChatRoom.ViewModel = function () {
    var self = this;
    self.isConnected = ko.observable(false);
    self.users = ko.observableArray();
    self.messages = ko.observableArray();
    self.sendMessageContent = ko.observable("");

    self.joins = function (username, joinedAt) {
        self.users.push(new ChatRoom.ConnectedUser(username, joinedAt));
    }

    self.leaves = function (username) {
        self.users.remove(function (item) {
            return item.username === username;
        });
    }

    self.initUsers = function (users) {
        var copyUsers = self.users().slice();

        self.users.removeAll();
        ko.utils.arrayForEach(users, function (item) {
            self.users.push(item);
        });

        ko.utils.arrayForEach(copyUsers, function (item) {
            self.users.push(item);
        });
    };

    self.getByUsername = function (username) {
        return ko.utils.arrayFirst(self.users(), function (user) {
            return user.username === username;
        });
    };

    self.onMessageReceived = function (username, message, timestamp) {
        self.messages.push(new ChatRoom.ChatMessage(username, message, timestamp));
    }

    self.initMessages = function (messages) {
        var copyMessages = self.messages.slice();

        self.messages.removeAll();
        ko.utils.arrayForEach(messages, function (item) {
            self.messages.push(item);
        });

        ko.utils.arrayForEach(copyMessages, function (item) {
            self.messages.push(item);
        });
    }

    self.sendMessage = function () {
        if (self.sendMessageContent() !== "") {
            $.connection.chatHub.server.send(self.sendMessageContent()).done(function () {
                self.sendMessageContent("");
            }).fail(function (e) {
                alert("Could not connect to server");
            });
        }
    }

    self.logout = function () {
        $.connection.chatHub.server.logout().done(function () {
            $.connection.hub.stop();
        });
        return true;
    }

    self.throttledSendMessageContent = ko.computed(self.sendMessageContent)
                                         .extend({ throttle: ChatRoom.Throttle_Send_Message });

    self.throttledSendMessageContent.subscribe(function (message) {
        $.connection.chatHub.server.typing(message);
    });
}

ChatRoom.documentReady = function (username, usernameQueryStringKey) {
    $.connection.hub.qs = usernameQueryStringKey + "=" + username;
    var client = $.connection.chatHub.client;
    var viewModel = new ChatRoom.ViewModel();

    client.connectionFailed = function (message) {
        alert("ConnectionFailed:" + message);
    }

    client.joins = function (username, joinedAt) {
        viewModel.joins(username, joinedAt);
    }

    client.leaves = function (username, timestamp) {
        viewModel.leaves(username);
    }

    client.connectionSucceed = function (usersDto, messagesDto) {
        viewModel.initUsers($.map(usersDto, function (item) {
            return new ChatRoom.ConnectedUser(item.Username, item.JoinedAt);
        }));
        viewModel.initMessages($.map(messagesDto, function (item) {
            return new ChatRoom.ChatMessage(item.Username, item.Content, item.Timestamp);
        }));
        viewModel.isConnected(true);
    }

    client.onMessageReceived = function (username, message, timestamp) {
        viewModel.getByUsername(username).onTyping("");
        viewModel.onMessageReceived(username, message, timestamp);
    }

    client.onTyping = function (username, message) {
        viewModel.getByUsername(username).onTyping(message);
    }

    $.connection.hub.start()
        .done(function () {
            $.connection.chatHub.server.connect();
            ko.applyBindings(viewModel);
        });
};