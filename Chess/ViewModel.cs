using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class ViewModel
    {
        public Board СhessBoard { set; get; }

        public ViewModel()
        {
            SetupBoard();
        }

        private void SetupBoard()
        {
            СhessBoard = new Board();
            СhessBoard[0, 0] = ChessFigure.BlackRook;
        }
    }
}
