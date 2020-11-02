using BlazorServerApp_Chess.Models;
using BlazorServerApp_Chess.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BlazorServerApp_Chess.Hubs
{
    public class GameHub : Hub
    {
        public static List<PlayerModel> players = new List<PlayerModel>();
        public static HashSet<string> connectionIds = new HashSet<string>();
        public static GameService gameService;

        public GameHub()
        {

        }

        public override async Task OnConnectedAsync()
        {
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
            await GetAllPlayers(players);
        }

        public async Task GetAllPlayers(List<PlayerModel> pl)
        {
            Console.WriteLine("GetAllPlayers");
            Console.WriteLine(players.Count);
            foreach(var p in players)
            {
                Console.WriteLine($"{p.Username}");
            }
            
            await Clients.All.SendAsync("ReceiveAllPlayers", pl);
        }

        public async Task SendPlayerMove(string c, int row, int col)
        {
            Console.WriteLine("OnSendPlayerMove");
            await Clients.All.SendAsync("ReceivePlayerMove", c, row, col);
        }

        public async Task DeletePlayer(string id)
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

            Console.WriteLine("Deleted player");
            await GetAllPlayers(players);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            connectionIds.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
