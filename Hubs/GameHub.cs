using Microsoft.AspNetCore.SignalR;
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
            Console.WriteLine("Connected to GameHub");
        }
    }
}
