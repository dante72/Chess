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
        public BoardVM СhessBoard { set; get; }
        public ObservableCollection<CellVM> Cells { set; get; }

        private Figure selectedFigure;

        public Figure SelectedFigure
        {
            set
            {
                selectedFigure = value;

                selectedFigure?.GetCorrectPossibleMoves()
                    .Select(i => СhessBoard[i.Row, i.Column])
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
                    AI2.CreateTreePossibleMovies(СhessBoard.board, ref AI2.Head);
                    var newHead = AI2.Head.ChildNodes.First(i => i.Data.Board == СhessBoard.board);
                    var move = AI2.GetResult(AI2.Head, 4);
                    //var som = Task.Run(() => AI.GetNextMove2(СhessBoard.board, new CancellationTokenSource()));
                    // var some = AI.GetNextMove2(СhessBoard.board, new CancellationTokenSource());
                    // var some = som.Result;

                    //if (СhessBoard.board == СhessBoard.board)
                    //     MessageBox.Show("true");
                    //  else
                    //    MessageBox.Show("false");
                    // var some = СhessBoard.board.Cells.First(i => i == AI.GetCell(СhessBoard.board).Cell);

                    move.Figure.MoveTo(move.Cell);
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
                    СhessBoard[i, j].IsMarked = false;
        }
        public ViewModel()
        {
            СhessBoard = new BoardVM();
            //AI.CalculateStart2(СhessBoard.board);
        }
    }


}
