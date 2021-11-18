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
            СhessBoard[0, 0].ChessFigure = ChessFigure.BlackRook;
            СhessBoard[0, 1].ChessFigure = ChessFigure.BlackKnight;
            СhessBoard[0, 2].ChessFigure = ChessFigure.BlackBishop;
            СhessBoard[0, 3].ChessFigure = ChessFigure.BlackQueen;
            СhessBoard[0, 4].ChessFigure = ChessFigure.BlackKing;
            СhessBoard[0, 5].ChessFigure = ChessFigure.BlackBishop;
            СhessBoard[0, 6].ChessFigure = ChessFigure.BlackKnight;
            СhessBoard[0, 7].ChessFigure = ChessFigure.BlackRook;
            for (int i = 0; i < 8; i++)
            {
                СhessBoard[1, i].ChessFigure = ChessFigure.BlackPawn;
                СhessBoard[6, i].ChessFigure = ChessFigure.WhitePawn;
            }
            СhessBoard[7, 0].ChessFigure = ChessFigure.WhiteRook;
            СhessBoard[7, 1].ChessFigure = ChessFigure.WhiteKnight;
            СhessBoard[7, 2].ChessFigure = ChessFigure.WhiteBishop;
            СhessBoard[7, 3].ChessFigure = ChessFigure.WhiteQueen;
            СhessBoard[7, 4].ChessFigure = ChessFigure.WhiteKing;
            СhessBoard[7, 5].ChessFigure = ChessFigure.WhiteBishop;
            СhessBoard[7, 6].ChessFigure = ChessFigure.WhiteKnight;
            СhessBoard[7, 7].ChessFigure = ChessFigure.WhiteRook;
        }
    }
}
