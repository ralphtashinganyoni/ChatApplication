using ChatApplication.Server.Domain.DTOs.Messages;
using ChatApplication.Server.Domain.Entities;
using ChatApplication.Server.Domain.Interfaces;

namespace ChatApplication.Server.SignalR
{
    using Microsoft.AspNetCore.SignalR;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task SendMessage(string senderId, string receiverId, string content)
        {
            var message = await _chatService.SaveMessageAsync(senderId, receiverId, content);

            // Send the message to the sender and receiver
            await Clients.Users(senderId).SendAsync("ReceiveMessage", message);
            await Clients.Users(receiverId).SendAsync("ReceiveMessage", message);

        }

        public async Task LoadMessages(string senderId, string receiverId)
        {
            var messages = await _chatService.GetConversationAsync(senderId, receiverId);

            Console.WriteLine(JsonSerializer.Serialize(messages));

            // Send the messages back to the client
            await Clients.Caller.SendAsync("LoadMessages", messages);
        }
    }

}
