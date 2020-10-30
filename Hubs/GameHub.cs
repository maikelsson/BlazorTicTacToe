using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp_Chess.Hubs
{
    public class GameHub : Hub
    {

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} connected to GameHub!");
            await base.OnConnectedAsync();
        }

        public async Task SendPlayerMove(string[,] arr)
        {
            Console.WriteLine("OnSendPlayerMove");
            await Clients.All.SendAsync("ReceivePlayerMove", arr);
        }
    }
}
