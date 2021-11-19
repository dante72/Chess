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
        public Board СhessBoard { set; get; } = new Board();
        public ObservableCollection<Cell> Cells { set; get; }

        private Cell selectedItem;
        public Cell SelectedItem
        {
            set
            {
                selectedItem = value;
            }
            get => selectedItem;
        
        }

        public ViewModel()
        {
            SetupСhessBoard();
        }

        private void SetupСhessBoard()
        {
            СhessBoard[0, 0] = Figure.BlackRook;
            СhessBoard[0, 1] = Figure.BlackKnight;
            СhessBoard[0, 2] = Figure.BlackBishop;
            СhessBoard[0, 3] = Figure.BlackQueen;
            СhessBoard[0, 4] = Figure.BlackKing;
            СhessBoard[0, 5] = Figure.BlackBishop;
            СhessBoard[0, 6] = Figure.BlackKnight;
            СhessBoard[0, 7] = Figure.BlackRook;
            for (int i = 0; i < 8; i++)
            {
                СhessBoard[1, i] = Figure.BlackPawn;
                СhessBoard[6, i] = Figure.WhitePawn;
            }
            СhessBoard[7, 0] = Figure.WhiteRook;
            СhessBoard[7, 1] = Figure.WhiteKnight;
            СhessBoard[7, 2] = Figure.WhiteBishop;
            СhessBoard[7, 3] = Figure.WhiteQueen;
            СhessBoard[7, 4] = Figure.WhiteKing;
            СhessBoard[7, 5] = Figure.WhiteBishop;
            СhessBoard[7, 6] = Figure.WhiteKnight;
            СhessBoard[7, 7] = Figure.WhiteRook;
        }
    }
}
