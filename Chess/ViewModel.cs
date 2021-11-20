using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.Figure;

namespace Chess
{
    public class ViewModel : NotifyPropertyChanged
    {
        public BoardVM СhessBoard { set; get; } = new BoardVM();
        public ObservableCollection<CellVM> Cells { set; get; }

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
                
                if (value.IsMarked == false)
                    selectedItem?.Figure?.GetPossibleMoves().Select(i => СhessBoard[i.Row, i.Column]).ToList()
                        .ForEach(a => a.IsMarked = false);

                if (value?.IsMarked == true)
                { 
                    selectedItem?.Figure?.GetPossibleMoves().Select(i => СhessBoard[i.Row, i.Column]).ToList()
                        .ForEach(a => a.IsMarked = false);
                    value.Figure = selectedItem.Figure;
                    selectedItem.Figure = null;

                }
                else
                {
                    selectedItem = value;
                    selectedItem?.Figure?.GetPossibleMoves().Select(i => СhessBoard[i.Row, i.Column]).ToList()
                    .ForEach(a => a.IsMarked = true);

                    selectedItem.IsSelected = true;
                }
            }
            get => selectedItem;
        
        }

        public ViewModel()
        {
            SetupСhessBoard();
        }

        private void SetupСhessBoard()
        {
            СhessBoard[0, 0].Figure = new Rook(FigureColors.Black);
            СhessBoard[0, 1].Figure = new Knight(FigureColors.Black);
            СhessBoard[0, 2].Figure = new Bishop(FigureColors.Black);
            СhessBoard[0, 3].Figure = new Queen(FigureColors.Black);
            СhessBoard[0, 4].Figure = new King(FigureColors.Black);
            СhessBoard[0, 5].Figure = new Bishop(FigureColors.Black);
            СhessBoard[0, 6].Figure = new Knight(FigureColors.Black);
            СhessBoard[0, 7].Figure = new Rook(FigureColors.Black);
            for (int i = 0; i < 8; i++)
            {
                СhessBoard[1, i].Figure = new Pawn(FigureColors.Black);
                СhessBoard[6, i].Figure = new Pawn(FigureColors.White);
            }
            СhessBoard[7, 0].Figure = new Rook(FigureColors.White);
            СhessBoard[7, 1].Figure = new Knight(FigureColors.White);
            СhessBoard[7, 2].Figure = new Bishop(FigureColors.White);
            СhessBoard[7, 3].Figure = new Queen(FigureColors.White);
            СhessBoard[7, 4].Figure = new King(FigureColors.White);
            СhessBoard[7, 5].Figure = new Bishop(FigureColors.White);
            СhessBoard[7, 6].Figure = new Knight(FigureColors.White);
            СhessBoard[7, 7].Figure = new Rook(FigureColors.White);
        }
    }
}
