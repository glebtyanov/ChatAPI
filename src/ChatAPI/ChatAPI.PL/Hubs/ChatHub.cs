using ChatAPI.BLL.Interfaces;
using ChatAPI.BLL.Services;
using ChatAPI.DAL.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatAPI.PL.Hubs
{
    public sealed class ChatHub(IChatsService chatsService) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("OnConnected", $"{Context.ConnectionId}");
        }

        public async Task SendMessage(int chatId, int userId, string message)
        {
            var msg = await chatsService.SendMessageAsync(chatId, userId, message);
            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", msg.Author?.Name, msg.Text);
        }
        
        public async Task JoinChat(int chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined the chat.");
        }

        public async Task LeaveChat(int chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has left the chat.");
        }
        
        public async Task Create(string name, int userId)
        {
            var chat = new Chat() { Name = name, AdminId = userId };
            
            await chatsService.CreateAsync(chat);

            await Groups.AddToGroupAsync(Context.ConnectionId, chat.Name);
        }
    }
}