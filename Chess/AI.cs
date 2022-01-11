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
        public Figure Figure { get; set; }
        public Cell Cell { get; set; }
        public Board Board { get; set; }
        public float Score = 0;
    }

    public static class AI
    {
        static Random rnd = new Random();
        public static TreeNode Head;

        public static void GrowTreePossibleMovies(TreeNode head, int depth, TreeNode mainNode = null, int currentDepth = 0)
        {
            if (mainNode == null)
                mainNode = head;
            if (head.ChildNodes != null && depth > 0)
                foreach (var node in head.ChildNodes)
            {
                if (node.ChildNodes == null && depth > 0)
                {
                    CreateTreePossibleMoves(node, depth - currentDepth - 1, mainNode);
                }
                else
                {
                    GrowTreePossibleMovies(node, depth, mainNode,currentDepth + 1);
                }
            }

        }

        static public async Task Grow(TreeNode head, int depth)
        {
            var nodes = new ConcurrentBag<TreeNode>(head.ChildNodes);
            for (int i = 0; i < 4; i++)
                await Task.Run(() => Grow(nodes, depth));
        }

        static public void Grow(ConcurrentBag<TreeNode> bag, int depth)
        {
            while (bag.Count > 0)
            {
                TreeNode node;
                if (bag.TryTake(out node))
                    GrowTreePossibleMovies(node, depth);
            }
        }


    public static void CreateTreePossibleMoves(TreeNode head, int depth, TreeNode mainNode = null)
        {
            if (mainNode == null)
                mainNode = head;
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
            {
                head.ChildNodes.OrderByDescending(i => i.Data.Score);

            }
            else
            {
                head.ChildNodes.OrderBy(i => i.Data.Score);
            }



            foreach (var node in head.ChildNodes)
            {
                if (node.Data.Board.IsCheckMate)
                    {
                        if (node.Data.Board.Index % 2 == 0)
                            node.Data.Score += 9999;
                        else
                            node.Data.Score -= 9999;

                    if (mainNode.Data.Board.Index % 2 == 0 && mainNode.Data.Score > node.Data.Score)
                        mainNode.Data.Score = node.Data.Score;

                    if (mainNode.Data.Board.Index % 2 != 0 && mainNode.Data.Score < node.Data.Score)
                        mainNode.Data.Score = node.Data.Score;
                    break;
                    }

                if (mainNode.Data.Board.Index % 2 == 0 && mainNode.Data.Score > node.Data.Score)
                    mainNode.Data.Score = node.Data.Score;

                if (mainNode.Data.Board.Index % 2 != 0 && mainNode.Data.Score < node.Data.Score)
                    mainNode.Data.Score = node.Data.Score;

                if (depth > 0)
                    CreateTreePossibleMoves(node, depth - 1, mainNode);
            }
        }
        
        public static float FindMove(TreeNode head, int depth)
        {
            float res;
            if (head.ChildNodes == null || depth == 0)
                return head.Data.Score;
            
            
            if (head.Data.Board.Index % 2 != 0)
            {
                res =  head.ChildNodes.Max(i => FindMove(i, depth - 1));
            }
            else
            {
                res = head.ChildNodes.Min(i => FindMove(i, depth - 1));
            }
            
            return res * 0.9f;   
        }

        public static IASimple2 GetResult1(TreeNode head, int depth)
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

        public static IASimple2 GetResult(TreeNode head, int depth)
        {
            List<Task> tasks = new List<Task>();
            var dictionary = head.ChildNodes.ToDictionary(node => node, node => node.Data.Score);
            float minmax = head.Data.Board.Index % 2 != 0 ? (float)dictionary.Max(d => d.Value) : (float)dictionary.Min(d => d.Value);
            string str = "";
            foreach (var item in dictionary)
                str += $"{string.Format("{0, 15}", item.Key.Data.Figure)}\t{item.Value}\t{item.Key.Data.Cell}\n";
            MessageBox.Show(str);
            return dictionary.First(d => (float)d.Value == minmax).Key.Data;
        }
    }
}
