using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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

    public class IASimple2
    {
        public Figure Figure { get; set; }
        public Cell Cell { get; set; }
        public Board Board { get; set; }
        public float Score = 0;
    }

    public  static class AI2
    {
        public static TreeNode Head;

         public static void CreateTreePossibleMovies(TreeNode head, int maxDepth, int currentDepth = 0)
        {
            var figurs = head.Data.Board.Cells.Where(i => i.Figure != null).Select(i => i.Figure);

            foreach (var figure in figurs)
            {
                var movies = figure.GetCorrectPossibleMoves();

                foreach (var move in movies)
                {

                    var newBoard = new Board(head.Data.Board, figure.Position, move);
                    var node = new TreeNode()
                    {
                        Data = new IASimple2()
                        {
                            Figure = figure,
                            Cell = move,
                            Board = newBoard,
                            Score = move.Figure != null ? (move.Figure.Color == FigureColors.Black ? -1 : 1) * move.Figure.Weight : 0
                        } 
                    };

                    head.Add(node);
                    if (currentDepth < maxDepth)
                        CreateTreePossibleMovies(node, maxDepth, currentDepth + 1);
                }
            }
        }

        public static void CorrectTreePossibleMovies(TreeNode head, int maxDepth, int currentDepth = 0)
        {
            if (currentDepth >= maxDepth)
                return;

            if (head.ChildNodes == null)
            {
                CreateTreePossibleMovies(head, maxDepth, currentDepth + 1);
            }
            else
            {
                foreach (var node in head.ChildNodes)
                    CorrectTreePossibleMovies(node, maxDepth, currentDepth + 1);
            }
                    
        }



        public static float FindMax(TreeNode head, int depth, float sum = 0, int currentDepth = 0)
        {
            currentDepth++;
            if (head.ChildNodes == null || currentDepth > depth)
                return 0;
            
            sum += head.ChildNodes.Max(node => FindMax(node, depth, sum, currentDepth));
            sum += head.Data.Score;
            //float max = head.ChildNodes.Max(i => i.Data.Score);
            return sum;
        }

        public static void PrintNode(TreeNode head)
        {
            string str = "";
            str += head.Data.Board.ToString();
            str += "\n\n";
            foreach (var node in head.ChildNodes)
                str += $"{node.Data.Board}\n\n";

            MessageBox.Show(str);
        }

        public static IASimple2 GetResult(TreeNode head, int depth)
        {
            var dictionary = head.ChildNodes.ToDictionary(node => node, node => FindMax(node, depth));
            float max = dictionary.Max(d => d.Value);
            return dictionary.First(d => d.Value == max).Key.Data;
        }

    }
    public  static class AI
    {
        //private static CancellationTokenSource cancel;
        public static FigureColors Colors = FigureColors.Black;
        static public Dictionary<Board, IASimple> dictionary = new Dictionary<Board, IASimple>();
        public static IASimple GetNextMove(Board board)
        {
            var tasks = new List<Task>();
            var currentState = new List<IASimple>();
            var figurs = board.Cells.Where(i => i.Figure != null).Select(i => i.Figure);

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
                if (board.Index < 3)
                {
                    var newBoard = new Board(board, it.Figure.Position, it.Cell);

                    if (it.Cell.Figure != null)
                    {
                        it.Score = (it.Figure.Color == FigureColors.Black ? -1.0f : 1.0f) * (float)it.Cell.Figure.Weight / (board.Index % 2 != 0 ? board.Index : board.Index + 1);
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
                    if (item == null)
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
            /*if (board.Index == 1)
            {
                string str = "";
                foreach (var it in currentState)
                {
                    str += $"{it.Figure}\t{it.Cell.Row}\t{it.Cell.Column}\t{it.Score}\n";
                }
                MessageBox.Show(str);
            }*/
            return currentState.FirstOrDefault(i => i.Score == max);
        }

        public static List<Board> GetBoards(Board board)
        {
            //SelectedFigure.MoveTo(selectedItem.Value);
            //some.Figure.MoveTo(some.Cell);
            var figurs = board.Cells.Where(i => i.Figure != null).Select(i => i.Figure);
            List<Board> boards = new List<Board>();
            foreach (var figure in figurs)
            {
                var moves = figure.GetAllPossibleMoves();
                if (moves.Count > 0)
                foreach (Cell move in moves)
                {
                        boards.Add(new Board(board, figure.Position, move));
                }
            }

            return boards;
        }

        public async static void CalculateStart2(Board board)
        {
            var boards = GetBoards(board);
            foreach (Board b in boards)
                dictionary.Add(b, await Task.Run(() => GetNextMove2(b, new CancellationTokenSource())));
                    
        }

        public static IASimple GetCell(Board board)
        {
            var answers = dictionary.Select(i => i.Value).ToList();
            //var b = dictionary.First(i => i.Key == board);   
            return dictionary.First(i => i.Key == board).Value;
        }

        public static void Restart(Board board)
        {
            dictionary.Clear();
            CalculateStart2(board);
        }

    }
}
