using BlazorServerApp_Chess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp_Chess.Models
{
    public class GamePiece
    {
        public PieceStyle Style;

        public GamePiece()
        {
            Style = PieceStyle.Blank;
        }

    }
}
