
using Blazored.SessionStorage;
using BlazorServerApp_Chess.Data;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
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
        private IConnectionManager _connectionManager;

        public ChatHub(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public override async Task OnConnectedAsync()
        {
            if (!connectionIds.Contains(Context.ConnectionId))
            {
                connectionIds.Add(Context.ConnectionId);
                await Clients.All.SendAsync("ReceiveUsersCount", connectionIds.Count);
            }
            
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            connectionIds.Remove(Context.ConnectionId);
            Clients.All.SendAsync("ReceiveUsersCount", connectionIds.Count);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
            
        }

        public async Task SendMessageToOthers(string user)
        {
            string message = $"{user} connected to chat!";
            await Clients.Others.SendAsync("ReceiveMessage", user, message);
        }

        public async Task<int> GetAllActiveConnections()
        {
            return await Task.FromResult(connectionIds.Count);
        }
    }
}
