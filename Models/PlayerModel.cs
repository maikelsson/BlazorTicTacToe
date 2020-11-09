using BlazorServerApp_Chess.Enums;

namespace BlazorServerApp_Chess.Models
{
    public class PlayerModel
    {
        public string Username { get; set; }
        public string ConnectionId { get; set; }
        public PieceStyle CurrentSide { get; set; }
        public bool ConnectedFirst { get; set; }
        public bool IsPlayerTurn { get; set; }

        public bool IsReady = false;

        public PlayerModel(string connectionId = "", string username = "", bool connectedFirst = false)
        {
            ConnectionId = connectionId;
            Username = username;
            ConnectedFirst = connectedFirst;
            IsPlayerTurn = ConnectedFirst;
            CurrentSide = ConnectedFirst == true ? PieceStyle.X : PieceStyle.O;
        }
        
    }
}
