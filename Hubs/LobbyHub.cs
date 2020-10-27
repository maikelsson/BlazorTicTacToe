using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp_Chess.Hubs
{
    public class LobbyHub : Hub
    {

        public static HashSet<string> connectionIds = new HashSet<string>();

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine("On lobby hub");

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
            Console.WriteLine($"Hubconnection closed, connections count: {connectionIds.Count}");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
