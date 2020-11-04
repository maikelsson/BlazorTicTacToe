using BlazorServerApp_Chess.Models;
using BlazorServerApp_Chess.Services;
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
        public static List<PlayerModel> players = new List<PlayerModel>();
        public static HashSet<string> connectionIds = new HashSet<string>();
        public PlayerModel player;

        public bool joinedSecond;

        public override async Task OnConnectedAsync()
        {

            if (Context.ConnectionId == null) return;
            if (players.Count == 2) return; //room full

            if (players.Count == 1) // Receive info from opponent if connected second
            {
                var other = players.First();
                await Clients.Caller.SendAsync("ReceivePlayerInformation", other.ConnectionId, other.Username, other.IsReady);
            }

            Console.WriteLine($"{Context.ConnectionId} connected to GameHub!");
            connectionIds.Add(Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public async Task AddPlayer(string name)
        {
            PlayerModel player;
            lock (players)
            {
                var nameExists = players.Any(x => x.Username == name);
                if (nameExists)
                {
                    Random rnd = new Random();
                    name += rnd.Next(1, 100);
                }

                player = new PlayerModel(Context.ConnectionId, name);
                players.Add(player);
            }

            Console.WriteLine("Added player");
            await SendPlayerInformation(player.ConnectionId, player.Username, player.IsReady);

        }

        public async Task SendPlayerInformation(string id, string userName, bool isready)
        {
            //Sends player info to other user
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceivePlayerInformation", id, userName, isready);
        }

        public async Task SendPlayerMove(int col, int row)
        {
            await Clients.All.SendAsync("ReceivePlayerClickedEmptySpace", col, row);
        }

        public void DeletePlayer(string id)
        {
            PlayerModel player;
            lock (players)
            {
                var idExists = players.Any(x => x.ConnectionId == id);
                if (idExists)
                {
                    player = players.Find(x => x.ConnectionId == id);
                    players.Remove(player);
                }
            }

            Console.WriteLine($"Deleted player --- players count: {players.Count}");
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            DeletePlayer(Context.ConnectionId);
            connectionIds.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
