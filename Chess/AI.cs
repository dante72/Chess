using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static Chess.Figure;

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
                tasks.Add(Task.Run(() => GetNextMove2(newBoard, new CancellationTokenSource(), it, currentState)));
            }
            Task.WaitAll(tasks.ToArray());
            //tasks.ForEach(i => i)
            float max = currentState.Max(i => i.Score);

            return currentState.First(i => i.Score == max);
        }


        public static IASimple GetNextMove2(Board board, CancellationTokenSource cancel, IASimple item = null, List<IASimple> listCancel = null)
        {
            if (board.Index > 3 && item.Score <= 0)
                return item;

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

            if (listCancel == null)
                listCancel = currentState;

            foreach (var it in currentState)
            {
                if (board.Index < 5)
                {
                    var newBoard = new Board(board, it.Figure.Position, it.Cell);
                    newBoard.Index = board.Index + 1;
                    if (it.Cell.Figure != null)
                    {
                        it.Score = (it.Figure.Color == FigureColors.Black ? -1 : 1) * (float)it.Cell.Figure.Weight / board.Index;
                    }
                    //var fig = newBoard.Cells.Where(i => i.Figure != null && i.Figure.Color == it.Figure.Color).Select(i => i.Figure).ToList();
                    //var figEnemy = newBoard.Cells.Where(i => i.Figure != null && i.Figure.Color != it.Figure.Color).Select(i => i.Figure).ToList();

                    //if (fig.Any(i => i.GetAllPossibleMoves().Any(j => j.Row == it.Cell.Row && j.Column == it.Cell.Column)))
                    //it.Score *= newBoard.Index;
                    //if (figEnemy.Any(i => i.GetCorrectPossibleMoves().Any(j => j.Row == it.Cell.Row && j.Column == it.Cell.Column)))
                    //it.Score /= newBoard.Index;
                    /*var fi = board.Cells.Where(i => i.Figure != null).Select(i => i.Figure).ToList();
                    if (fi.Any(i => i.GetAllPossibleMoves().Contains(it.Cell)))
                    {
                        it.Score *= 2f;
                    }*/
                    if (board.Index == 1)
                        item = it;

                    lock (listCancel)
                    {
                        item.Score += it.Score;

                        //if (listCancel.Max(i => i.Score) - it.Score > 0.7f)
                            //return it;
                    }
                    try {
                        token.ThrowIfCancellationRequested();
                    }
                    catch { 
                        return item;
                    }
                   var res =  GetNextMove2(newBoard, cancel, item, listCancel);

                    it.Score += res.Score;
                }
            }
            float m = currentState.Max(i => i.Score);
            float max = board.Index % 2 == 0 ? m : currentState.Min(i => i.Score);
            if (board.Index == 1)
            {
                string str = "";
                foreach (var it in currentState)
                {
                    str += $"{it.Figure}\t{it.Cell.Row}\t{it.Cell.Column}\t{it.Score}\n";
                }
                MessageBox.Show(str);
            }
            return currentState.FirstOrDefault(i => i.Score == max);
        }

    }
}
