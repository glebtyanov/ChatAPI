using ChatAPI.BLL.Interfaces;
using ChatAPI.DAL.Models;
using ChatAPI.PL;
using ChatAPI.PL.Hubs;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace UnitTests.Hubs
{
    public class ChatHubTests
    {
        private readonly ChatHub _chatHub;
        private readonly Mock<IChatsService> _chatsServiceMock;
        private readonly Mock<IChatClient> _clientMock;
        private readonly Mock<IHubCallerClients<IChatClient>> _clientsMock;
        private readonly HubCallerContext _contextMock;
        private readonly Mock<IGroupManager> _groupsMock;
        private readonly Mock<IUsersService> _usersServiceMock;

        public ChatHubTests()
        {
            _chatsServiceMock = new Mock<IChatsService>();
            _usersServiceMock = new Mock<IUsersService>();
            _clientsMock = new Mock<IHubCallerClients<IChatClient>>();
            _clientMock = new Mock<IChatClient>();
            _groupsMock = new Mock<IGroupManager>();
            _contextMock = Mock.Of<HubCallerContext>(_ => _.ConnectionId == "test-connection-id");

            _clientsMock.Setup(clients => clients.Caller).Returns(_clientMock.Object);
            _clientsMock.Setup(clients => clients.Group(It.IsAny<string>())).Returns(_clientMock.Object);
            _clientsMock.Setup(clients => clients.OthersInGroup(It.IsAny<string>())).Returns(_clientMock.Object);

            _chatHub = new ChatHub(_chatsServiceMock.Object, _usersServiceMock.Object)
            {
                Clients = _clientsMock.Object,
                Context = _contextMock,
                Groups = _groupsMock.Object
            };
        }

        [Fact]
        public async Task OnConnectedAsync_ShouldAddConnection()
        {
            // Act
            await _chatHub.OnConnectedAsync();

            // Assert
            Assert.Contains(ChatHub.Connections, c => c.ConnectionId == _contextMock.ConnectionId);
        }

        [Fact]
        public async Task Connect_ShouldSetConnectionUserIdAndUsername()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, Name = "test-user" };
            _usersServiceMock.Setup(service => service.GetAsync(userId)).ReturnsAsync(user);

            // Act
            await _chatHub.Connect(userId);

            // Assert
            var connection = ChatHub.Connections.First();
            Assert.Equal(userId, connection.UserId);
            Assert.Equal("test-user", connection.Username);
        }

        [Fact]
        public async Task JoinChat_ShouldAddToGroup()
        {
            // Arrange
            ChatHub.Connections.Add(new ConnectionViewModel
                { ConnectionId = "test-connection-id", Username = "test-user" });
            var chat = new Chat { Id = 1, Name = "test-chat" };
            _chatsServiceMock.Setup(service => service.GetByNameAsync("test-chat")).ReturnsAsync(chat);

            // Act
            await _chatHub.JoinChat("test-chat");

            // Assert
            _groupsMock.Verify(groups =>
                    groups.AddToGroupAsync("test-connection-id", "test-chat", default),
                Times.Once);
            _clientsMock.Verify(clients =>
                    clients.Group("test-chat").ReceiveFromServer("test-user has joined"),
                Times.Once);
        }

        [Fact]
        public async Task RemoveChat_ShouldRemoveChatIfAdmin()
        {
            // Arrange
            var connection = new ConnectionViewModel
                { ConnectionId = "test-connection-id", ChatName = "test-chat", UserId = 1 };
            ChatHub.Connections.Add(connection);
            var chat = new Chat { Id = 1, Name = "test-chat", AdminId = 1 };
            _chatsServiceMock.Setup(service => service.GetByNameAsync("test-chat")).ReturnsAsync(chat);

            // Act
            await _chatHub.RemoveChat("test-chat");

            // Assert
            _chatsServiceMock.Verify(service => service.RemoveAsync(1, 1), Times.Once);
            _clientsMock.Verify(clients =>
                clients.OthersInGroup("test-chat").ReceiveFromServer("You have been removed"), Times.Once);
            _clientsMock.Verify(clients =>
                clients.Caller.ReceiveFromServer("Chat test-chat has been successfully removed"), Times.Once);
            Assert.Equal(string.Empty, connection.ChatName);
            Assert.Equal(default, connection.ChatId);
        }

        [Fact]
        public async Task RemoveChat_ShouldNotRemoveChatIfNotAdmin()
        {
            // Arrange
            var connection = new ConnectionViewModel
                { ConnectionId = "test-connection-id", ChatName = "test-chat", UserId = 2 };
            ChatHub.Connections.Add(connection);
            var chat = new Chat { Id = 1, Name = "test-chat", AdminId = 1 };
            _chatsServiceMock.Setup(service => service.GetByNameAsync("test-chat")).ReturnsAsync(chat);

            // Act
            await _chatHub.RemoveChat("test-chat");

            // Assert
            _clientsMock.Verify(clients =>
                clients.Caller.ReceiveFromServer("Only admin of this chat can remove it"), Times.Once);
        }

        [Fact]
        public async Task CreateChat_ShouldCreateChat()
        {
            // Arrange
            var connection = new ConnectionViewModel
                { ConnectionId = "test-connection-id", UserId = 1, Username = "test-user" };
            ChatHub.Connections.Add(connection);
            var chat = new Chat { Name = "test-chat", AdminId = 1 };
            _chatsServiceMock.Setup(service => service.CreateAsync(It.IsAny<Chat>())).ReturnsAsync(chat);

            // Act
            await _chatHub.CreateChat("test-chat");

            // Assert
            _chatsServiceMock.Verify(service => service.CreateAsync(It.IsAny<Chat>()), Times.Once);
            _clientsMock.Verify(clients =>
                clients.Caller.ReceiveFromServer("test-user created chat test-chat"), Times.Once);
        }

        [Fact]
        public async Task RemoveChatOnServer_ShouldRemoveChat()
        {
            // Arrange
            var connection = new ConnectionViewModel
                { ConnectionId = "test-connection-id", ChatName = "test-chat", UserId = 1 };
            ChatHub.Connections.Add(connection);
            var chat = new Chat { Id = 1, Name = "test-chat", AdminId = 1 };
            _chatsServiceMock.Setup(service => service.GetByNameAsync("test-chat")).ReturnsAsync(chat);

            // Act
            await _chatHub.RemoveChatOnServer("test-chat");

            // Assert
            _chatsServiceMock.Verify(service => service.RemoveAsync(1, 1), Times.Once);
            _clientsMock.Verify(
                clients => clients.Group("test-chat").ReceiveFromServer("Chat test-chat has been removed"), Times.Once);
            Assert.Equal(string.Empty, connection.ChatName);
            Assert.Equal(default, connection.ChatId);
        }
    }
}