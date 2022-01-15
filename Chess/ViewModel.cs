using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static Chess.Figure;

namespace Chess
{
    public class ViewModel : NotifyPropertyChanged
    {
        private Board currentBoard;
        public List<string> Letters { set; get; } = new List<string>() { "a", "b", "c", "d", "e", "f", "g", "h"};
        public List<string> Digits { set; get; } = new List<string>() { "8", "7", "6", "5", "4", "3", "2", "1" };
        public BoardVM ChessBoard { set; get; }

        private Figure selectedFigure;

        public Figure SelectedFigure
        {
            set
            {
                selectedFigure = value;

                selectedFigure?.GetPossibleMoves()
                    .Select(i => ChessBoard[i.Row, i.Column])
                    .ToList()
                    .ForEach(a => a.IsMarked = true);
            }
            get 
            {
                return selectedFigure;
            }
        }

        private CellVM selectedItem;
        
        /// <summary>
        /// Выбранная клетка на доске (вся локика доски)
        /// </summary>
        public CellVM SelectedItem
        {
            set
            {
                if (selectedItem != null)
                    selectedItem.IsSelected = false;

                selectedItem = value;

                if (selectedItem.IsMarked)
                {
                    SelectedFigure.MoveTo(selectedItem.Value);
                    if (ChessBoard.Board.IsCheckMate)
                    {
                        MessageBox.Show("MATE!");
                        ChessBoard.Update(currentBoard);
                    }
                    else if (ChessBoard.Board.isCheckPate())
                    {
                        MessageBox.Show("PATE!");
                        ChessBoard.Update(currentBoard);
                    }

                    if (ChessBoard.Board.Moves >= 0 && ChessBoard.Board.Moves <= ChessBoard.Board.Index - 1)
                    {
                        MessageBox.Show("Moves are over");
                        ChessBoard.Update(currentBoard);
                    }
                    selectedItem.IsSelected = false;

                    //превращение пешки
                    if (SelectedFigure is Pawn p && (selectedItem.Value.Row == 0 || selectedItem.Value.Row == 7))
                    {
                        PawnTransform dialog = new PawnTransform(p.Color);
                        dialog.ShowDialog();
                        SelectedFigure.Position.Figure = (Figure)dialog.DataContext;
                    }
                    
                }
                
                ClearMarks();
                SelectedFigure = selectedItem?.Figure;
                   

                selectedItem.IsSelected = true;
            }
            get
            {
                return selectedItem;
            }
        
        }

        private void ClearMarks()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    ChessBoard[i, j].IsMarked = false;
        }
        public ViewModel()
        {
            ChessBoard = new BoardVM();
            currentBoard = new Board(ChessBoard.Board);
            //var f = EvaluateBoard.PawnEvalBlack;
            //var g = EvaluateBoard.PawnEvalWhite;
            //MessageBox.Show($"{EvaluateBoard.Print(f)}\n{EvaluateBoard.Print(g)}");

        }

        private RelayCommand makeMoveCommand;
        public RelayCommand MakeMoveCommand
        {
            get
            {
                return makeMoveCommand ??
                    (makeMoveCommand = new RelayCommand(obj =>
                    {
                        if (AI.Head == null)
                        {
                            AI.Head = new TreeNode();
                            AI.Head.Data = new IASimple2 { Board = new Board(ChessBoard.Board) };
                            for (int i = 0; i < 3; i++)
                                AI.CreateTreePossibleMoves(AI.Head, 3);
                        }
                        else
                            AI.CreateTreePossibleMoves(AI.Head, 3);
                        var move = AI.GetResult(AI.Head, 3);
                        
                        ChessBoard.Board[move.Figure.Position.Row, move.Figure.Position.Column].Figure.MoveTo(ChessBoard.Board[move.Cell.Row, move.Cell.Column]);
                        AI.Head = AI.Head.ChildNodes.First(i => i.Data.Board == ChessBoard.Board);
                        AI.Head.Parent = null;
                        if (ChessBoard.Board.IsCheckMate)
                        {
                            MessageBox.Show("MATE!");
                            ChessBoard.Update(currentBoard);
                        }
                        else if (ChessBoard.Board.isCheckPate())
                        {
                            MessageBox.Show("PATE!");
                            ChessBoard.Update(currentBoard);
                        }

                        if (ChessBoard.Board.Moves >= 0 && ChessBoard.Board.Moves <= ChessBoard.Board.Index - 1)
                        {
                            MessageBox.Show("Moves are over");
                            ChessBoard.Update(currentBoard);
                        }
                    }));
            }


        }

        private RelayCommand chessTasksCommand;
        public RelayCommand ChessTasksCommand
        {
            get
            {
                return chessTasksCommand ??
                    (chessTasksCommand = new RelayCommand(obj =>
                    {
                        var dialog = new ChessTasks();
                        if (dialog.ShowDialog() == true)
                        {
                            var board = (Board)dialog.DataContext;
                            ChessBoard.Update(board);
                            currentBoard = new Board(board);
                        }
                    }));
            }
        }
    }
}
