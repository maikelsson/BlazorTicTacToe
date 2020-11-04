using BlazorServerApp_Chess.Models;
using System.Collections.Generic;

namespace BlazorServerApp_Chess.Services
{
    public interface IGameService
    {
        GameBoard gameBoard { get; }
        List<PlayerModel> players { get; }

    }
}