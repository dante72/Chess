using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class IASimple
    {
        public Figure Figure { get; set; }
        public Cell Cell { get; set; }

        public float Score { get; set; }
    }
    public static class AI
    {
        public static FigureColors Colors = FigureColors.Black;
        public static IASimple GetNextMove(Board board)
        {
            var figurs = board.Cells.Where(i => i.Figure != null).Select(i => i.Figure);

            var listAI = new List<IASimple>();
            foreach (var figure in figurs)
            {
                var movies = figure.GetCorrectPossibleMoves();

                foreach (var move in movies)
                {
                    var item = new IASimple { Figure = figure, Cell = move };

                    if (move.Figure != null)
                        item.Score += move.Figure.Weight;

                    listAI.Add(item);
                }
                    
            }

            foreach (var it in listAI)
            {
                var newBoard = new Board(board, it.Figure.Position, it.Cell);
                newBoard.Index = board.Index + 1;
                if (newBoard.Index < 4)
                    it.Score = GetNextMove(newBoard).Score;
            }
            float max = listAI.Max(i => i.Score);
                
            return listAI.First(j => j.Score == max);
        }
    }
}
