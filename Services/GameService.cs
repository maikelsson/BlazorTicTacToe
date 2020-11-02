using BlazorServerApp_Chess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp_Chess.Services
{
    public class GameService
    {
        public GameBoard gameBoard;
        public List<PlayerModel> players { get; set; }
        public GameService()
        {
            gameBoard = new GameBoard();
            players = new List<PlayerModel>();
        }
    }
}
