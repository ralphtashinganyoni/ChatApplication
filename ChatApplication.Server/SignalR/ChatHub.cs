using ChatApplication.Server.Domain.DTOs.Messages;
using ChatApplication.Server.Domain.Entities;
using ChatApplication.Server.Domain.Interfaces;

namespace ChatApplication.Server.SignalR
{
    using ChatApplication.Server.Infrastructure.Services;
    using Microsoft.AspNetCore.SignalR;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly IHttpUserContextService _httpUserContextService;

        public ChatHub(IChatService chatService, IHttpUserContextService httpUserContextService)
        {
            _chatService = chatService;
            _httpUserContextService = httpUserContextService;
        }

        private static readonly Dictionary<string, string> UserConnections = new Dictionary<string, string>();

        /// <summary>
        /// Called when a client connects to the hub.
        /// </summary>
        /// <returns>A Task that represents the asynchronous operation.</returns>
        public override Task OnConnectedAsync()
        {
            var userId = GetUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                lock (UserConnections)
                {
                    UserConnections[userId] = Context.ConnectionId;
                }
            }

            return base.OnConnectedAsync();
        }

        /// <summary>
        /// Called when a client disconnects from the hub.
        /// </summary>
        /// <param name="exception">The exception that caused the disconnect, if any.</param>
        /// <returns>A Task that represents the asynchronous operation.</returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userId = GetUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                lock (UserConnections)
                {
                    UserConnections.Remove(userId);
                }
            }

            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Sends a message from the current user to the specified receiver using connection IDs.
        /// </summary>
        /// <param name="receiverId">The receiver's user ID.</param>
        /// <param name="content">The content of the message.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task SendMessage(string receiverId, string content)
        {
            var senderId = GetUserId();
            if (string.IsNullOrEmpty(senderId))
            {
                throw new HubException("Unable to identify the sender.");
            }

            var message = await _chatService.SaveMessageAsync(senderId, receiverId, content);

            // Send the message to the sender
            if (UserConnections.TryGetValue(senderId, out var senderConnectionId))
            {
                await Clients.Client(senderConnectionId).SendAsync("ReceiveMessage", message);
            }

            // Send the message to the receiver
            if (UserConnections.TryGetValue(receiverId, out var receiverConnectionId))
            {
                await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", message);
            }
        }

        /// <summary>
        /// Loads the conversation between the current user and the specified receiver using connection IDs.
        /// </summary>
        /// <param name="receiverId">The receiver's user ID.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task LoadMessages(string receiverId)
        {
            var senderId = GetUserId();
            if (string.IsNullOrEmpty(senderId))
            {
                throw new HubException("Unable to identify the sender.");
            }

            var messages = await _chatService.GetConversationAsync(senderId, receiverId);

            // Send the messages to the sender
            if (UserConnections.TryGetValue(senderId, out var senderConnectionId))
            {
                await Clients.Client(senderConnectionId).SendAsync("LoadMessages", messages);
            }
        }

        public string? GetUserId()
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext == null)
            {
                Console.WriteLine("HttpContext is null.");
                return null;
            }

            // Retrieve the user ID from headers or query string
            var userId = httpContext.Request.Headers["UserId"].FirstOrDefault() ??
                         httpContext.Request.Query["userId"].FirstOrDefault();

            Console.WriteLine($"Retrieved User ID: {userId}");
            return userId;
        }


    }

}
