using System.Linq.Expressions;
using ChatAPI.BLL.Exceptions;
using ChatAPI.BLL.Services;
using ChatAPI.DAL.Interfaces;
using ChatAPI.DAL.Models;
using Moq;

namespace UnitTests.Services
{
    public class ChatsServiceTests
    {
        private readonly Mock<IChatsRepository> _chatsRepositoryMock;
        private readonly ChatsService _chatsService;
        private readonly Mock<IMessagesRepository> _messagesRepositoryMock;
        private readonly Mock<IUsersRepository> _usersRepositoryMock;

        public ChatsServiceTests()
        {
            _chatsRepositoryMock = new Mock<IChatsRepository>();
            _messagesRepositoryMock = new Mock<IMessagesRepository>();
            _usersRepositoryMock = new Mock<IUsersRepository>();
            _chatsService = new ChatsService(_chatsRepositoryMock.Object, _messagesRepositoryMock.Object,
                _usersRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowBadRequestException_WhenChatWithNameExists()
        {
            // Arrange
            var chat = new Chat { Name = "TestChat" };
            _chatsRepositoryMock.Setup(repo =>
                repo.ExistsAsync(It.IsAny<Expression<Func<Chat, bool>>>())).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _chatsService.CreateAsync(chat));
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnChat_WhenChatIsCreatedSuccessfully()
        {
            // Arrange
            var chat = new Chat { Name = "TestChat" };
            _chatsRepositoryMock.Setup(repo =>
                repo.ExistsAsync(It.IsAny<Expression<Func<Chat, bool>>>())).ReturnsAsync(false);

            // Act
            var result = await _chatsService.CreateAsync(chat);

            // Assert
            Assert.Equal(chat, result);
            _chatsRepositoryMock.Verify(repo => repo.CreateAsync(chat), Times.Once);
        }

        [Fact]
        public async Task GetWhereUserIsAdminAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            _usersRepositoryMock.Setup(repo =>
                repo.ExistsAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _chatsService.GetWhereUserIsAdminAsync(userId));
        }

        [Fact]
        public async Task GetWhereUserIsAdminAsync_ShouldReturnChats_WhenUserIsAdmin()
        {
            // Arrange
            var userId = 1;
            var chats = new List<Chat> { new() { AdminId = userId } };
            _usersRepositoryMock.Setup(repo =>
                repo.ExistsAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(true);
            _chatsRepositoryMock.Setup(repo =>
                repo.GetWhereUserIsAdminAsync(userId)).ReturnsAsync(chats);

            // Act
            var result = await _chatsService.GetWhereUserIsAdminAsync(userId);

            // Assert
            Assert.Equal(chats, result);
        }

        [Fact]
        public async Task GetAsync_ShouldThrowNotFoundException_WhenChatDoesNotExist()
        {
            // Arrange
            var chatId = 1;
            _chatsRepositoryMock.Setup(repo =>
                repo.GetByIdAsync(chatId)).ReturnsAsync((Chat)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _chatsService.GetAsync(chatId));
        }

        [Fact]
        public async Task GetAsync_ShouldReturnChat_WhenChatExists()
        {
            // Arrange
            var chatId = 1;
            var chat = new Chat { Id = chatId };
            _chatsRepositoryMock.Setup(repo => repo.GetByIdAsync(chatId)).ReturnsAsync(chat);

            // Act
            var result = await _chatsService.GetAsync(chatId);

            // Assert
            Assert.Equal(chat, result);
        }

        [Fact]
        public async Task RemoveAsync_ShouldThrowNotFoundException_WhenChatDoesNotExist()
        {
            // Arrange
            var chatId = 1;
            var userId = 1;
            _chatsRepositoryMock.Setup(repo =>
                repo.GetByIdAsync(chatId)).ReturnsAsync((Chat)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _chatsService.RemoveAsync(chatId, userId));
        }

        [Fact]
        public async Task RemoveAsync_ShouldThrowBadRequestException_WhenUserIsNotAdmin()
        {
            // Arrange
            var chatId = 1;
            var userId = 2;
            var chat = new Chat { Id = chatId, AdminId = 1 };
            _chatsRepositoryMock.Setup(repo => repo.GetByIdAsync(chatId)).ReturnsAsync(chat);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _chatsService.RemoveAsync(chatId, userId));
        }

        [Fact]
        public async Task RemoveAsync_ShouldRemoveChat_WhenUserIsAdmin()
        {
            // Arrange
            var chatId = 1;
            var userId = 1;
            var chat = new Chat { Id = chatId, AdminId = userId };
            _chatsRepositoryMock.Setup(repo => repo.GetByIdAsync(chatId)).ReturnsAsync(chat);

            // Act
            await _chatsService.RemoveAsync(chatId, userId);

            // Assert
            _chatsRepositoryMock.Verify(repo => repo.RemoveAsync(chat), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowNotFoundException_WhenChatDoesNotExist()
        {
            // Arrange
            var chatId = 1;
            var updatedChat = new Chat { Id = chatId };
            _chatsRepositoryMock.Setup(repo =>
                repo.GetByIdAsync(chatId)).ReturnsAsync((Chat)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _chatsService.UpdateAsync(chatId, updatedChat));
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedChat_WhenChatExists()
        {
            // Arrange
            var chatId = 1;
            var existingChat = new Chat { Id = chatId };
            var updatedChat = new Chat { Id = chatId, Name = "UpdatedName" };
            _chatsRepositoryMock.Setup(repo =>
                repo.GetByIdAsync(chatId)).ReturnsAsync(existingChat);

            // Act
            var result = await _chatsService.UpdateAsync(chatId, updatedChat);

            // Assert
            Assert.Equal(updatedChat, result);
            _chatsRepositoryMock.Verify(repo => repo.UpdateAsync(updatedChat), Times.Once);
        }

        [Fact]
        public async Task GetByNameAsync_ShouldThrowNotFoundException_WhenChatDoesNotExist()
        {
            // Arrange
            var chatName = "TestChat";
            _chatsRepositoryMock.Setup(repo =>
                repo.GetWhereAsync(It.IsAny<Expression<Func<Chat, bool>>>())).ReturnsAsync((Chat)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _chatsService.GetByNameAsync(chatName));
        }

        [Fact]
        public async Task GetByNameAsync_ShouldReturnChat_WhenChatExists()
        {
            // Arrange
            var chatName = "TestChat";
            var chat = new Chat { Name = chatName };
            _chatsRepositoryMock.Setup(repo =>
                repo.GetWhereAsync(It.IsAny<Expression<Func<Chat, bool>>>())).ReturnsAsync(chat);

            // Act
            var result = await _chatsService.GetByNameAsync(chatName);

            // Assert
            Assert.Equal(chat, result);
        }

        [Fact]
        public async Task ExistsWithNameAsync_ShouldReturnTrue_WhenChatWithNameExists()
        {
            // Arrange
            var chatName = "TestChat";
            _chatsRepositoryMock.Setup(repo =>
                repo.ExistsAsync(It.IsAny<Expression<Func<Chat, bool>>>())).ReturnsAsync(true);

            // Act
            var result = await _chatsService.ExistsWithNameAsync(chatName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExistsWithNameAsync_ShouldReturnFalse_WhenChatWithNameDoesNotExist()
        {
            // Arrange
            var chatName = "TestChat";
            _chatsRepositoryMock.Setup(repo =>
                repo.ExistsAsync(It.IsAny<Expression<Func<Chat, bool>>>())).ReturnsAsync(false);

            // Act
            var result = await _chatsService.ExistsWithNameAsync(chatName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task SendMessageAsync_ShouldReturnMessage_WhenMessageIsCreatedSuccessfully()
        {
            // Arrange
            var chatId = 1;
            var authorId = 1;
            var text = "Hello!";
            var message = new Message { ChatId = chatId, AuthorId = authorId, Text = text };
            _messagesRepositoryMock.Setup(repo =>
                repo.CreateAsync(message)).Returns(Task.CompletedTask);
            _messagesRepositoryMock.Setup(repo =>
                repo.GetByIdAsync(message.Id)).ReturnsAsync(message);

            // Act
            var result = await _chatsService.SendMessageAsync(chatId, authorId, text);

            // Assert
            Assert.Equal(message, result);
        }
    }
}