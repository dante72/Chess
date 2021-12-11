using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Chess
{
    public class ViewModel : NotifyPropertyChanged
    {
        public List<string> Letters { set; get; } = new List<string>() { "a", "b", "c", "d", "e", "f", "g", "h"};
        public List<string> Digits { set; get; } = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8" };
        public BoardVM ChessBoard { set; get; }
        public ObservableCollection<CellVM> Cells { set; get; }

        private Figure selectedFigure;

        public Figure SelectedFigure
        {
            set
            {
                selectedFigure = value;

                selectedFigure?.GetCorrectPossibleMoves()
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
                    //AI2.CreateTreePossibleMovies(СhessBoard.board, ref AI2.Head);
                    AI2.Head = AI2.Head.ChildNodes.First(i => i.Data.Board == ChessBoard.board);
                    var move = AI2.GetResult(AI2.Head, 4);
                    ChessBoard.board[move.Figure.Position.Row, move.Figure.Position.Column].Figure.MoveTo(ChessBoard.board[move.Cell.Row, move.Cell.Column]);

                    //move.Figure.MoveTo(move.Cell);
                    AI2.Head = AI2.Head.ChildNodes.First(i => i.Data.Board == ChessBoard.board);
                    //var nodes = .SelectMany(i => i.ChildNodes);
                   
                    //var som = Task.Run(() => AI.GetNextMove2(СhessBoard.board, new CancellationTokenSource()));
                    // var some = AI.GetNextMove2(СhessBoard.board, new CancellationTokenSource());
                    // var some = som.Result;

                    //if (СhessBoard.board == СhessBoard.board)
                    //     MessageBox.Show("true");
                    //  else
                    //    MessageBox.Show("false");
                    // var some = СhessBoard.board.Cells.First(i => i == AI.GetCell(СhessBoard.board).Cell);

                    
                    //AI.Restart(СhessBoard.board);
                    selectedItem.IsSelected = false;
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
            AI2.CreateTreePossibleMovies(ChessBoard.board, ref AI2.Head);

        }
    }


}
