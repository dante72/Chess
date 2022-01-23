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
        public bool SingleMode { get; set; } = false;

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
                    //ход игрока
                    UpdateMovementTree();
                    SelectedFigure.MoveTo(selectedItem.Value);
                    UpdateHead();
                    if(!CheckBoard())
                        return;

                    selectedItem.IsSelected = false;

                    //если игрок сходил, ход ИИ
                    if (SingleMode && ChessBoard.Board.Index % 2 == 0)
                    {
                        UpdateMovementTree();
                        var move = AI.GetResult(AI.Head, 2);
                        ChessBoard.Board[move.Figure.Position.Row, move.Figure.Position.Column].Figure.MoveTo(ChessBoard.Board[move.Cell.Row, move.Cell.Column]);
                        UpdateHead();
                        CheckBoard();
                    }

                    PawnTransform();
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

        private void PawnTransform()
        {
            //превращение пешки
            if (SelectedFigure is Pawn p && (selectedItem.Value.Row == 0 || selectedItem.Value.Row == 7))
            {
                PawnTransform dialog = new PawnTransform(p.Color);
                dialog.ShowDialog();
                SelectedFigure.Position.Figure = (Figure)dialog.DataContext;
            }
        }
        private bool CheckBoard()
        {
            bool flag = false;
            if (ChessBoard.Board.IsCheckMate)
            {
                MessageBox.Show("MATE!");
                ChessBoard.Update(currentBoard);
                ClearMarks();
            }
            else if (ChessBoard.Board.isCheckPate())
            {
                MessageBox.Show("PATE!");
                ChessBoard.Update(currentBoard);
                ClearMarks();
            }
            else if (ChessBoard.Board.Moves >= 0 && ChessBoard.Board.Moves <= ChessBoard.Board.Index - 1)
            {
                MessageBox.Show("Moves are over");
                ChessBoard.Update(currentBoard);
                ClearMarks();
            }
            else
                flag = true;
            return flag;
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
            currentBoard = new Board();
        }

        private RelayCommand makeMoveCommand;
        public RelayCommand MakeMoveCommand
        {
            get
            {
                return makeMoveCommand ??
                    (makeMoveCommand = new RelayCommand(obj =>
                    {
                        ClearMarks();
                        UpdateMovementTree();
                        var move = AI.GetResult(AI.Head, 2);
                        ChessBoard.Board[move.Figure.Position.Row, move.Figure.Position.Column].Figure.MoveTo(ChessBoard.Board[move.Cell.Row, move.Cell.Column]);
                        if (!CheckBoard())
                            return;
                        UpdateHead();

                        if (SingleMode)
                        {
                            UpdateMovementTree();
                            move = AI.GetResult(AI.Head, 2);
                            ChessBoard.Board[move.Figure.Position.Row, move.Figure.Position.Column].Figure.MoveTo(ChessBoard.Board[move.Cell.Row, move.Cell.Column]);
                            CheckBoard();
                            UpdateHead();
                        }
                    }));
            }

        }

        private void UpdateMovementTree()
        {
            if (AI.Head == null)
            {
                AI.Head = new TreeNode();
                AI.Head.Data = new IASimple { Board = new Board(ChessBoard.Board) };
                AI.CreateTreePossibleMoves(AI.Head, 2);
            }
            else
            {
                AI.GrowTreePossibleMoves(AI.Head, 2);
            }
        }

        private void UpdateHead()
        {
            AI.Head = AI.Head.ChildNodes.First(b => b.Data.Board == ChessBoard.Board);
        }

        private RelayCommand chessTasksCommand;
        public RelayCommand ChessTasksCommand
        {
            get
            {
                return chessTasksCommand ??
                    (chessTasksCommand = new RelayCommand(obj =>
                    {
                        ClearMarks();
                        var dialog = new ChessTasks();
                        if (dialog.ShowDialog() == true)
                        {
                            var board = dialog.Value;
                            ChessBoard.Update(board);
                            currentBoard = new Board(board);
                            SingleMode = true;
                        }
                    }));
            }
        }

        private RelayCommand moveBackCommand;
        public RelayCommand MoveBackCommand
        {
            get
            {
                return moveBackCommand ??
                    (moveBackCommand = new RelayCommand(obj =>
                    {
                        AI.Head = null;
                        ChessBoard.Board.MoveBack();
                    }));
            }
        }

        private RelayCommand singlePlayerCommand;
        public RelayCommand SinglePlayerCommand
        {
            get
            {
                return singlePlayerCommand ??
                    (singlePlayerCommand = new RelayCommand(obj =>
                    {
                        ClearMarks();
                        SingleMode = true;
                        ChessBoard.Update(new Board());
                    }));
            }
        }

        private RelayCommand multiPlayerCommand;
        public RelayCommand MultiPlayerCommand
        {
            get
            {
                return multiPlayerCommand ??
                    (multiPlayerCommand = new RelayCommand(obj =>
                    {
                        ClearMarks();
                        SingleMode = false;
                        ChessBoard.Update(new Board());
                    }));
            }
        }
    }
}
