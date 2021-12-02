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

        public float Score = 0;
    }
    public static class AI
    {
        public static FigureColors Colors = FigureColors.Black;
        public static IASimple GetNextMove(Board board)
        {

            var figurs = board.Cells.Where(i => i.Figure != null).Select(i => i.Figure);

            var currentState = new List<IASimple>();
            foreach (var figure in figurs)
            {
                var movies = figure.GetCorrectPossibleMoves();

                foreach (var move in movies)
                {
                    var item = new IASimple { Figure = figure, Cell = move};

                    currentState.Add(item);
                }

            }

            foreach (var it in currentState)
            {
                var newBoard = new Board(board, it.Figure.Position, it.Cell);
                newBoard.Index = board.Index + 1;

                if (it.Cell.Figure != null)
                {
                    if (it.Cell.Figure.Color != it.Figure.Color)
                        it.Score += (float)it.Cell.Figure.Weight / board.Index;
                    else
                        it.Score -= (float)it.Cell.Figure.Weight / board.Index;
                }

                it.Score += GetNextMove2(newBoard, it);
            }

            float max = currentState.Max(i => i.Score);

            return currentState.First(i => i.Score == max);
        }


        static float GetNextMove2(Board board, IASimple item)
        {

            var figurs = board.Cells.Where(i => i.Figure != null).Select(i => i.Figure);

            var currentState = new List<IASimple>();
            foreach (var figure in figurs)
            {
                var movies = figure.GetCorrectPossibleMoves();

                foreach (var move in movies)
                {
                    var item1 = new IASimple { Figure = figure, Cell = move };

                    currentState.Add(item1);
                }

            }



            foreach (var it in currentState)
            {
                if (board.Index < 4)
                {
                    var newBoard = new Board(board, it.Figure.Position, it.Cell);
                    newBoard.Index = board.Index + 1;
                    if (it.Cell.Figure != null)
                    {
                        if (it.Cell.Figure.Color != it.Figure.Color)
                            it.Score += (float)it.Cell.Figure.Weight / board.Index;
                        else
                            it.Score -= (float)it.Cell.Figure.Weight / board.Index;
                    }
                    it.Score += GetNextMove2(newBoard, it);
                }
            }
            float max = currentState.Max(i => i.Score);

            return max;
        }


    }
}
