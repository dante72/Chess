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
        public Board Board { get; set; }
        public float Score = 0;
    }

    public static class AI
    {
        public static TreeNode Head;

        public static void SearchTreeEndAndAdd(TreeNode head, int depth)
        {
            if (depth > 0 && head != null)
            {
                if (head.ChildNodes != null)
                    Parallel.ForEach(head.ChildNodes, node => SearchTreeEndAndAdd(node, depth - 1));
                else
                    Parallel.ForEach(head.ChildNodes, node => CreateTreePossibleMoves(node, depth - 1));
            }
        }

        public static void GrowTreePossibleMoves(TreeNode head, int depth, int currentDepth = 0)
        {
            if (currentDepth < depth)
            {
                if (head.ChildNodes == null)
                    CreateTreePossibleMoves(head, depth, currentDepth + 1);
                else
                    Parallel.ForEach(head.ChildNodes, node => GrowTreePossibleMoves(node, depth, currentDepth + 1));
            }
        }
        public static void CreateTreePossibleMoves(TreeNode head, int depth, int currentDepth = 0)
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
                        Data = new IASimple()
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
            
            if (currentDepth < depth && head.ChildNodes != null && !head.Data.Board.IsCheckMate)
                Parallel.ForEach(head.ChildNodes, node => CreateTreePossibleMoves(node, depth, currentDepth + 1));
        }
        
        public static float FindMove(TreeNode head, int depth, int currentDepth = 0)
        {
            float res;

            if (head.ChildNodes == null)
                return head.Data.Score;
            
            
            if (head.Data.Board.Index % 2 != 0)
            {
                res =  head.ChildNodes.Max(i => FindMove(i, depth, currentDepth + 1));
            }
            else
            {
                res = head.ChildNodes.Min(i => FindMove(i, depth, currentDepth + 1));
            }

            return res * 0.9f;   
        }

        public static IASimple GetResult(TreeNode head, int depth)
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
