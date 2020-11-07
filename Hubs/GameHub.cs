using BlazorServerApp_Chess.Enums;
using BlazorServerApp_Chess.Models;
using BlazorServerApp_Chess.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp_Chess.Hubs
{
    public class GameHub : Hub
    {
        public static Dictionary<string, HashSet<string>> GameGroups = new Dictionary<string, HashSet<string>>(); // Keeping track of the groups
        
        public static List<PlayerModel> players = new List<PlayerModel>();
        public static HashSet<string> connectionIds = new HashSet<string>();

        public override async Task OnConnectedAsync()
        {
            if (Context.ConnectionId == null) return;

            if (players.Count == 1) // Receive info from opponent if connected second
            {
                var other = players.First();
                await Clients.Caller.SendAsync("ReceiveOtherPlayerInformation", other.ConnectionId, other.Username);
            }

            Console.WriteLine($"{Context.ConnectionId} connected to GameHub!");
            connectionIds.Add(Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        //private async Task CreateOrJoinToGroupAsync()
        //{
        //    // Check for rooms where user count is less than 2, if none then create one
        //    if (GameGroups.Count == 0)
        //    {
        //        // If none, create one
        //        var roomName = $"room_{Context.ConnectionId}";
        //        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        //        GameGroups[roomName] = new HashSet<string>();
        //    }
        //    else
        //    {
        //        foreach (var group in GameGroups.Keys)
        //        {
        //            if (group.)
        //        }
        //    }

        //}

        public async Task AddPlayer(string name)
        {
            bool isFirst = false;
            PlayerModel player;
            
            lock (players)
            {
                var nameExists = players.Any(x => x.Username == name); // Check for duplicates
                if (nameExists)
                {
                    Random rnd = new Random();
                    name += rnd.Next(1, 100);
                }

                if (players.Count == 0)
                {
                    isFirst = true;
                }

                player = new PlayerModel(Context.ConnectionId, name, isFirst);

                players.Add(player);


            }

            Console.WriteLine($"Added player! {player.Username} : connectedfirst : {player.ConnectedFirst}");
            await SendMyPlayerInformation(player.ConnectionId, player.Username, player.CurrentSide);
            await SendOtherPlayerInformation(player.ConnectionId, player.Username);

        }

        public async Task SendMyPlayerInformation(string id, string username, PieceStyle pieceStyle)
        {
            await Clients.Caller.SendAsync("ReceiveMyPlayerInformation", id, username, pieceStyle);
        }

        public async Task SendOtherPlayerInformation(string id, string userName)
        {
            //Sends player info to other user
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveOtherPlayerInformation", id, userName);
        }

        public async Task SendPlayerMove(int col, int row, string id)
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
                    Console.WriteLine($"Deleted player: {player.ConnectionId} --- players count: {players.Count}");
                }
                else
                {
                    Console.WriteLine("Couldn't find user");
                }
            }

        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            DeletePlayer(Context.ConnectionId);
            connectionIds.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
