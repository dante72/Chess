using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public static class AI
    {
        public static Cell GetNextMove(Board board)
        {
            return board.Cells.First(i => i.Figure != null && i.Figure.Color == FigureColors.Black && i.Figure.GetCorrectPossibleMoves().Count > 0);
        }
    }
}
