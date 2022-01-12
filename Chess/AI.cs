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

    public class IASimple2 : IComparable
    {
        public Figure Figure { get; set; }
        public Cell Cell { get; set; }
        public Board Board { get; set; }
        public float Score = 0;

        public int CompareTo(object obj)
        {
            return (int)(Score - (obj as IASimple2).Score);
        }
    }

    public static class AI
    {
        public static TreeNode Head;

        public static void CreateTreePossibleMovies(TreeNode head, int depth, int currentDepth = 0)
        {
            var figurs = head.Data.Board.Cells.Where(i => i.Figure != null).Select(i => i.Figure);

            foreach (var figure in figurs)
            {
                var moves = figure.PossibleMoves;

                foreach (var move in moves)
                {
                    var newBoard = new Board(head.Data.Board, figure.Position, move);
                    
                    var node = new TreeNode()
                    {
                        Data = new IASimple2()
                        {
                            Figure = figure,
                            Cell = move,
                            Board = newBoard,
                            Score = newBoard.Evaluation()
                        }
                    };
                    
                    head.Add(node);
                }
            }
            if (head.ChildNodes == null)
                return;
            
            if (head.Data.Board.Index % 2 == 0)
                head.ChildNodes.OrderBy(i => i.Data.Score);
            else
                head.ChildNodes.OrderByDescending(i => i.Data.Score);

            foreach (var node in head.ChildNodes)
            {                    
                if (node.Data.Board.IsCheckMate)
                    {
                        if (node.Data.Board.Index % 2 == 0)
                            node.Data.Score += 9999;
                        else
                            node.Data.Score -= 9999;
                    }
                    else
                        if (currentDepth < depth)
                            CreateTreePossibleMovies(node, depth, currentDepth + 1);
            }
        }
        public static float FindMove(TreeNode head, int depth, int currentDepth = 0, IASimple2 mate = null)
        {
            if (mate == null)
                mate = new IASimple2();
            if (Math.Abs(head.Data.Score) > 5000)
            {
                mate.Score = head.Data.Score * (float)Math.Pow(0.9f, currentDepth + 1);
            }
            float res;

            if (head.ChildNodes == null)
                return head.Data.Score;
            
            
            if (head.Data.Board.Index % 2 != 0)
            {
                res =  head.ChildNodes.Max(i => FindMove(i, depth, currentDepth + 1, mate));
            }
            else
            {
                res = head.ChildNodes.Min(i => FindMove(i, depth, currentDepth + 1, mate));
            }


            if (currentDepth == 0 && Math.Abs(mate.Score) > Math.Abs(res))
                return mate.Score;
            return res * 0.9f;   
        }

        public static IASimple2 GetResult(TreeNode head, int depth)
        {
            List<Task> tasks = new List<Task>();
            var dictionary = head.ChildNodes.ToDictionary(node => node, node => FindMove(node, depth));
            float minmax = head.Data.Board.Index % 2 != 0 ? (float)dictionary.Max(d => d.Value) : (float)dictionary.Min(d => d.Value);
            string str = "";
            foreach (var item in dictionary)
                str += $"{string.Format("{0, 15}", item.Key.Data.Figure)}\t{item.Value}\t{item.Key.Data.Cell}\n";
            MessageBox.Show(str);
            return dictionary.First(d => (float)d.Value == minmax).Key.Data;
        }
    }
}
