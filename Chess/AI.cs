using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chess
{
    public class IASimple
    {
        public Figure Figure { get; set; }
        public Cell Cell { get; set; }

        public float Score = 0;
    }
    public  static class AI
    {
        //private static CancellationTokenSource cancel;
        public static FigureColors Colors = FigureColors.Black;
        public static IASimple GetNextMove(Board board)
        {
            var tasks = new List<Task>();

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
                //CancellationToken token = cancel.Token;
                //it.Score += GetNextMove2(newBoard, it).Result;
                tasks.Add(Task.Run(() => GetNextMove2(newBoard, it, currentState, new CancellationTokenSource())));
            }
            Task.WaitAll(tasks.ToArray());
            //tasks.ForEach(i => i)
            float max = currentState.Max(i => i.Score);

            return currentState.First(i => i.Score == max);
        }


        static async Task<float> GetNextMove2(Board board, IASimple item, List<IASimple> listCancel, CancellationTokenSource cancel)
        {

            CancellationToken token = cancel.Token;
            

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
                if (board.Index < 5)
                {
                    var newBoard = new Board(board, it.Figure.Position, it.Cell);
                    newBoard.Index = board.Index + 1;
                    if (it.Cell.Figure != null)
                    {
                        if (it.Figure.Color == FigureColors.Black)
                            it.Score += (float)it.Cell.Figure.Weight / board.Index;
                        else
                            it.Score -= (float)it.Cell.Figure.Weight;
                    }
                    /*var fig = newBoard.Cells.Where(i => i.Figure != null && i.Figure.Color == it.Figure.Color).Select(i => i.Figure).ToList();
                    var figEnemy = newBoard.Cells.Where(i => i.Figure != null && i.Figure.Color != it.Figure.Color).Select(i => i.Figure).ToList();
                    
                    if (fig.Any(i => i.GetAllPossibleMoves().Any(j => j.Row == it.Cell.Row && j.Column == it.Cell.Column)))
                        it.Score *= newBoard.Index;
                    if (figEnemy.Any(i => i.GetCorrectPossibleMoves().Any(j => j.Row == it.Cell.Row && j.Column == it.Cell.Column)))
                        it.Score /= newBoard.Index;*/

                    lock (listCancel)
                    {
                        item.Score = it.Score;

                        if (listCancel.Max(i => i.Score) - it.Score > 0.7f)
                            cancel.Cancel();
                    }
                    try {
                        token.ThrowIfCancellationRequested();
                    }
                    catch { 
                        return item.Score;
                    }
                    
                    it.Score += await GetNextMove2(newBoard, item, listCancel, cancel);
                }
            }
            float max = currentState.Max(i => i.Score);

            return max;
        }


    }
}
