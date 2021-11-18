using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class ViewModel
    {
        public Board СhessBoard { set; get; }
        public ObservableCollection<Cell> Cells { set; get; }

        private Cell selectedItem;
        public Cell SelectedItem
        {
            set
            {
                selectedItem = value;
                if (СhessBoard[0, 0] == selectedItem)
                    СhessBoard[1, 1].IsMarked = true;
            }
            get => selectedItem;
        
        }

        public ViewModel()
        {
            SetupBoard();
        }

        private void SetupBoard()
        {
            СhessBoard = new Board();
            Cells = new ObservableCollection<Cell>(СhessBoard);
            СhessBoard[0, 0].ChessFigure = ChessFigure.BlackRook;
        }
    }
}
