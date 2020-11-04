using BlazorServerApp_Chess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp_Chess.Models
{
    public class PlayerModel
    {
        public string Username { get; set; }
        public string ConnectionId { get; set; }
        public PieceStyle currentSide { get; set; }
        public bool IsReady = false;
        public bool IsPlayerTurn = false;
        public PlayerModel(string connectionId, string username)
        {
            ConnectionId = connectionId;
            Username = username;
            currentSide = PieceStyle.Blank;
        }
        
    }
}
