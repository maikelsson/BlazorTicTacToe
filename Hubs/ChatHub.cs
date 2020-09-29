using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp_Chess.Hubs
{
    public static class UserHandler
    {
    }

    public class ChatHub : Hub
    {

        public static HashSet<string> connectionIds = new HashSet<string>();

        public override Task OnConnectedAsync()
        {
            connectionIds.Add(Context.ConnectionId);
            GetAllActiveConnections();
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message, string time)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message, time);
        }

        public async Task SendMessageToOthers(string user)
        {
            string message = $"{user} connected to chat!";
            await Clients.Others.SendAsync("ReceiveMessage", user, message, DateTime.Now.ToShortTimeString());
        }

        public int GetAllActiveConnections()
        {
            return connectionIds.Count;
        }
    }
}
