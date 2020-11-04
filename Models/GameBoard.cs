using BlazorServerApp_Chess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp_Chess.Models
{
    public class GameBoard
    {
        public GamePiece[,] Board { get; set; }
        
        public PieceStyle CurrentTurn = PieceStyle.X;
        public GameBoard()
        {
            ResetBoard();
        }

        public void ResetBoard()
        {
            Board = new GamePiece[3, 3];

            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    Board[i, j] = new GamePiece();
                }
            }
        }

        public void PieceClicked(int x, int y)
        {
            GamePiece clickedSpace = Board[x, y];
            if(clickedSpace.Style == PieceStyle.Blank)
            {
                clickedSpace.Style = CurrentTurn;
                SwitchTurns();
                Console.WriteLine("cliced space" + Board.Length);
            }
        }

        private void SwitchTurns()
        {
            CurrentTurn = CurrentTurn == PieceStyle.X ? PieceStyle.O : PieceStyle.X;
        }
    }
}
