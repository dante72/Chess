using System;
using System.Collections.Concurrent;
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
        public bool IsCheckMate { get; set; } = false;
        public Figure Figure { get; set; }
        public Cell Cell { get; set; }
        public Board Board { get; set; }
        public float Score = 0;
    }

    public static class AI
    {
        public static TreeNode Head;

        /*public static void GrowTreePossibleMovies(TreeNode head, int depth, int currentDepth = 0)
        {
            if (head.ChildNodes != null && currentDepth < depth)
                foreach (var node in head.ChildNodes)
                {
                    if (node.ChildNodes == null && currentDepth < depth)
                    {
                        CreateTreePossibleMoves(node, depth, currentDepth + 1);
                    }
                    else
                    {
                        GrowTreePossibleMovies(node, depth, currentDepth + 1);
                    }
                }
        }*/

        /*static public async Task Grow(TreeNode head, int depth)
        {
            var nodes = new ConcurrentBag<TreeNode>(head.ChildNodes);
            for (int i = 0; i < 4; i++)
                await Task.Run(() => Grow(nodes, depth));
        }*/

        /*static public void Grow(ConcurrentBag<TreeNode> bag, int depth)
        {
            while (bag.Count > 0)
            {
                TreeNode node;
                if (bag.TryTake(out node))
                    lock(node)
                        GrowTreePossibleMovies(node, depth);
            }
        }*/

        public static void CreateTreePossibleMoves(TreeNode head, int depth)
        {
            if (depth > 0 && !head.Data.IsCheckMate)
            {
                if (head.ChildNodes == null)
                    CreateTreePossibleMoves(head);
                else
                    foreach (var node in head.ChildNodes)
                        CreateTreePossibleMoves(node, depth - 1);
            }
        }
        public static void CreateTreePossibleMoves(TreeNode head)
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
                            Score = newBoard.Evaluation(),
                            IsCheckMate = newBoard.IsCheckMate
                        }
                    };

                    head.Add(node);
                    if (node.Data.IsCheckMate)
                    {
                        MarkBranchWithMate(node);
                    }
                }
            }
        }

        static private void MarkBranchWithMate(TreeNode node)
        {
            if (node.Parent != null)
            {
                node.Parent.Data.Score = node.Data.Score * 0.9f;
                node.Parent.Data.IsCheckMate = true;
                MarkBranchWithMate(node.Parent);
            }
        }

        public static float FindMove(TreeNode head, int depth)
        {
            if (head.Data.IsCheckMate)
                return head.Data.Score;

            if (depth > 0 && head.ChildNodes != null && !head.Data.IsCheckMate)
            {
                float minmax;
                if (head.Data.Board.Index % 2 == 0)
                    minmax = head.ChildNodes.Max(i => i.Data.Score);
                else
                    minmax = head.ChildNodes.Max(i => i.Data.Score);

                return FindMove(head.ChildNodes.First(i => i.Data.Score == minmax), depth - 1);
            }

            return head.Data.Score;
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
