using System.Net;
using System.Text;
using ChatAPI.DAL.Data;
using ChatAPI.DAL.Models;
using ChatAPI.PL.DTO;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace IntegrationTests
{
    public class ChatsControllerTests(TestWebFactory factory) : IClassFixture<TestWebFactory>
    {
        private readonly HttpClient _client = factory.CreateClient();
        private readonly IServiceProvider _serviceProvider = factory.Services;

        [Fact]
        public async Task Create_ShouldReturnCreatedChat()
        {
            await ResetDatabaseAsync();
            // Arrange
            var userCreateDto = new UserCreateDto { Name = "Test User" };
            var userContent = new StringContent(JsonConvert.SerializeObject(userCreateDto), Encoding.UTF8,
                "application/json");

            var userResponse = await _client.PostAsync("api/Users/", userContent);
            var userId = JsonConvert.DeserializeObject<User>(await userResponse.Content.ReadAsStringAsync())!.Id;

            var chatCreateDto = new ChatCreateDto { Name = "Test Chat", AdminId = userId };
            var chatContent = new StringContent(JsonConvert.SerializeObject(chatCreateDto), Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/Chats", chatContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var chat = JsonConvert.DeserializeObject<Chat>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(chat);
            Assert.Equal("Test Chat", chat.Name);
        }

        [Fact]
        public async Task GetForUser_ShouldReturnChatsForUser()
        {
            await ResetDatabaseAsync();
            // Arrange
            var userCreateDto = new UserCreateDto { Name = "Test User" };
            var userContent = new StringContent(JsonConvert.SerializeObject(userCreateDto), Encoding.UTF8,
                "application/json");

            var userResponse = await _client.PostAsync("api/Users/", userContent);
            var userId = JsonConvert.DeserializeObject<User>(await userResponse.Content.ReadAsStringAsync())!.Id;

            var chatCreateDto = new ChatCreateDto { Name = "Test Chat", AdminId = userId };
            var chatContent = new StringContent(JsonConvert.SerializeObject(chatCreateDto), Encoding.UTF8,
                "application/json");

            await _client.PostAsync("/api/Chats", chatContent);

            // Act
            var response = await _client.GetAsync($"/api/Chats?userId={userId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var chats = JsonConvert.DeserializeObject<List<Chat>>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(chats);
            Assert.Single(chats);
            Assert.Equal("Test Chat", chats.First().Name);
        }

        [Fact]
        public async Task GetById_ShouldReturnChat()
        {
            await ResetDatabaseAsync();
            // Arrange
            var userCreateDto = new UserCreateDto { Name = "Test User" };
            var userContent = new StringContent(JsonConvert.SerializeObject(userCreateDto), Encoding.UTF8,
                "application/json");

            var userResponse = await _client.PostAsync("api/Users/", userContent);
            var userId = JsonConvert.DeserializeObject<User>(await userResponse.Content.ReadAsStringAsync())!.Id;

            var chatCreateDto = new ChatCreateDto { Name = "Test Chat", AdminId = userId };
            var chatContent = new StringContent(JsonConvert.SerializeObject(chatCreateDto), Encoding.UTF8,
                "application/json");

            var chatResponse = await _client.PostAsync("/api/Chats", chatContent);
            var chatId = JsonConvert.DeserializeObject<Chat>(await chatResponse.Content.ReadAsStringAsync())!.Id;

            // Act
            var response = await _client.GetAsync($"/api/Chats/{chatId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var chat = JsonConvert.DeserializeObject<Chat>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(chat);
            Assert.Equal("Test Chat", chat.Name);
        }

        [Fact]
        public async Task Update_ShouldReturnUpdatedChat()
        {
            await ResetDatabaseAsync();
            // Arrange
            var userCreateDto = new UserCreateDto { Name = "Test User" };
            var userContent = new StringContent(JsonConvert.SerializeObject(userCreateDto), Encoding.UTF8,
                "application/json");

            var userResponse = await _client.PostAsync("api/Users/", userContent);
            var userId = JsonConvert.DeserializeObject<User>(await userResponse.Content.ReadAsStringAsync())!.Id;

            var chatCreateDto = new ChatCreateDto { Name = "Test Chat", AdminId = userId };
            var chatContent = new StringContent(JsonConvert.SerializeObject(chatCreateDto), Encoding.UTF8,
                "application/json");

            var chatResponse = await _client.PostAsync("/api/Chats", chatContent);
            var chatId = JsonConvert.DeserializeObject<Chat>(await chatResponse.Content.ReadAsStringAsync())!.Id;

            var updatedChatDto = new ChatCreateDto { Name = "Updated Chat", AdminId = userId };
            var updatedChatContent = new StringContent(JsonConvert.SerializeObject(updatedChatDto), Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PutAsync($"/api/Chats/{chatId}", updatedChatContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var updatedChat = JsonConvert.DeserializeObject<Chat>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(updatedChat);
            Assert.Equal("Updated Chat", updatedChat.Name);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent()
        {
            await ResetDatabaseAsync();
            // Arrange
            var userCreateDto = new UserCreateDto { Name = "Test User" };
            var userContent = new StringContent(JsonConvert.SerializeObject(userCreateDto), Encoding.UTF8,
                "application/json");

            var userResponse = await _client.PostAsync("api/Users/", userContent);
            var userId = JsonConvert.DeserializeObject<User>(await userResponse.Content.ReadAsStringAsync())!.Id;

            var chatCreateDto = new ChatCreateDto { Name = "Test Chat", AdminId = userId };
            var chatContent = new StringContent(JsonConvert.SerializeObject(chatCreateDto), Encoding.UTF8,
                "application/json");

            var chatResponse = await _client.PostAsync("/api/Chats", chatContent);
            var chatId = JsonConvert.DeserializeObject<Chat>(await chatResponse.Content.ReadAsStringAsync())!.Id;

            // Act
            var response = await _client.DeleteAsync($"/api/Chats/{chatId}?userId={userId}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        private async Task ResetDatabaseAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
            }
        }
    }
}