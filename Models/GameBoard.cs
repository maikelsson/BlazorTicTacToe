using BlazorServerApp_Chess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp_Chess.Models
{
    public class GameBoard
    {
        public bool GameComplete => GetWinner() != null || IsADraw();
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

            CurrentTurn = PieceStyle.X;
        }

        public void PieceClicked(int x, int y)
        {
            if (GameComplete) return;

            GamePiece clickedSpace = Board[x, y];
            if(clickedSpace.Style == PieceStyle.Blank)
            {
                clickedSpace.Style = CurrentTurn;
                SwitchTurns();
            }
        }

        public bool IsADraw()
        {
            int blankPieceCount = 0;

            // counting all the blank pieces, if 0 black pieces then game is a draw
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    blankPieceCount = this.Board[i, j].Style == PieceStyle.Blank
                                        ? blankPieceCount + 1
                                        : blankPieceCount;
                }
            }

            return blankPieceCount == 0;
        }

        public WinningPlay GetWinner()
        {
            WinningPlay winningPlay = null;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    foreach (EvaluationDirection evalDirection in (EvaluationDirection[])Enum.GetValues(typeof(EvaluationDirection)))
                    {
                        winningPlay = EvaluatePieceForWinner(i, j, evalDirection);
                        if (winningPlay != null) 
                        { 
                            return winningPlay; 
                        }
                    }
                }
            }

            return winningPlay;
        }

        private WinningPlay EvaluatePieceForWinner(int i, int j, EvaluationDirection dir)
        {
            GamePiece currentPiece = Board[i, j];
            if (currentPiece.Style == PieceStyle.Blank)
            {
                return null;
            }

            int inARow = 1;
            int iNext = i;
            int jNext = j;

            var winningMoves = new List<string>();

            while (inARow < 3)
            {
                switch (dir)
                {
                    case EvaluationDirection.Up:
                        jNext -= 1;
                        break;
                    case EvaluationDirection.UpRight:
                        iNext += 1;
                        jNext -= 1;
                        break;
                    case EvaluationDirection.Right:
                        iNext += 1;
                        break;
                    case EvaluationDirection.DownRight:
                        iNext += 1;
                        jNext += 1;
                        break;
                }
                if (iNext < 0 || iNext >= 3 || jNext < 0 || jNext >= 3) { break; }
                if (Board[iNext, jNext].Style == currentPiece.Style)
                {
                    winningMoves.Add($"{iNext},{jNext}");
                    inARow++;
                }
                else
                {
                    return null;
                }
            }

            if (inARow >= 3)
            {
                winningMoves.Add($"{i},{j}");

                return new WinningPlay()
                {
                    WinningMoves = winningMoves,
                    WinningStyle = currentPiece.Style,
                    WinningDirection = dir,
                };
            }

            return null;
        }

        private void SwitchTurns()
        {
            CurrentTurn = CurrentTurn == PieceStyle.X ? PieceStyle.O : PieceStyle.X;
        }
    }
}
