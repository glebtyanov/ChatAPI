using ChatAPI.BLL.Interfaces;
using ChatAPI.DAL.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatAPI.PL.Hubs
{
    public sealed class ChatHub(IChatsService chatsService, IUsersService usersService) : Hub<IChatClient>
    {
        public static readonly List<ConnectionViewModel> Connections = [];

        private ConnectionViewModel Connection => Connections.First(c => c.ConnectionId == Context.ConnectionId);

        public override Task OnConnectedAsync()
        {
            Connections.Add(new ConnectionViewModel { ConnectionId = Context.ConnectionId });
            return Task.CompletedTask;
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await LeaveChat(Connection.ChatName);
        }

        // with identity the logic of this method would be in OnConnected
        public async Task Connect(int userId)
        {
            var user = await usersService.GetAsync(userId);

            Connection.UserId = userId;
            Connection.Username = user.Name;
        }

        public async Task SendMessage(string message)
        {
            var msg = await chatsService.SendMessageAsync(Connection.ChatId, Connection.UserId, message);
            await Clients.Group(Connection.ChatName).ReceiveFromUser(msg.Author?.Name!, msg.Text!);
        }

        public async Task JoinChat(string chatName)
        {
            var chat = await chatsService.GetByNameAsync(chatName);

            if (Connection.ChatName == chatName)
            {
                await Clients.Caller.ReceiveFromServer("You already joined this chat");
                return;
            }

            if (!string.IsNullOrEmpty(Connection.ChatName))
            {
                await LeaveChat(Connection.ChatName);
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, chatName);
            await Clients.Group(chatName).ReceiveFromServer($"{Connection.Username} has joined");

            Connection.ChatId = chat.Id;
            Connection.ChatName = chat.Name;
        }

        public async Task LeaveChat(string chatName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatName);
            await Clients.Caller.ReceiveFromServer($"You have left the chat {Connection.ChatName}");
            await Clients.OthersInGroup(chatName).ReceiveFromServer($"{Connection.Username} has left");

            Connection.ChatName = string.Empty;
            Connection.ChatId = default;
        }

        public async Task RemoveChat(string chatName)
        {
            var chatToRemove = await chatsService.GetByNameAsync(chatName);
            if (Connection.UserId != chatToRemove.AdminId)
            {
                await Clients.Caller.ReceiveFromServer("Only admin of this chat can remove it");
                return;
            }

            await chatsService.RemoveAsync(chatToRemove.Id, Connection.UserId);

            await Clients.OthersInGroup(chatName).ReceiveFromServer("You have been removed");

            foreach (var connection in Connections.Where(c => c.ChatName == chatName))
            {
                await Groups.RemoveFromGroupAsync(connection.ConnectionId, chatName);
                connection.ChatName = string.Empty;
                connection.ChatId = default;
            }

            await Clients.Caller.ReceiveFromServer($"Chat {chatName} has been successfully removed");
        }

        public async Task RemoveChatOnServer(Chat chat)
        {
            if (Connections.All(c => c.ChatName != chat.Name))
            {
                return;
            }

            await Clients.Group(chat.Name).ReceiveFromServer($"Chat {chat.Name} has been removed");

            var connections = Connections.Where(c => c.ChatName == chat.Name).ToList();

            foreach (var connection in connections)
            {
                await Groups.RemoveFromGroupAsync(connection.ConnectionId, chat.Name);
                connection.ChatName = string.Empty;
                connection.ChatId = default;
            }
        }

        public async Task CreateChat(string chatName)
        {
            var chat = new Chat { Name = chatName, AdminId = Connection.UserId };

            await chatsService.CreateAsync(chat);

            await Clients.Caller.ReceiveFromServer($"{Connection.Username} created chat {chatName}");
        }
    }
}