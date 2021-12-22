using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    /// <summary>
    /// Доска для ViewModel
    /// </summary>
    public class BoardVM : IEnumerable<CellVM>
    {
        private readonly CellVM[,] cells;

        public readonly Board board;

        public CellVM this[int row, int column]
        {
            get => cells[row, column];
            private set
            {
                cells[row, column] = value;  
            }
        }

        public BoardVM()
        {
            board = new Board();
            cells = new CellVM[8, 8];
            for (int i = 0; i < cells.GetLength(0); i++)
                for (int j = 0; j < cells.GetLength(1); j++)
                    cells[i, j] = new CellVM(board[i, j]);
        }


        public BoardVM(Board board)
        {
            this.board = board;
            cells = new CellVM[8, 8];
            for (int i = 0; i < cells.GetLength(0); i++)
                for (int j = 0; j < cells.GetLength(1); j++)
                    cells[i, j] = new CellVM(board[i, j]);
        }

        public IEnumerator<CellVM> GetEnumerator()
            => cells.Cast<CellVM>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => cells.GetEnumerator();
    }
}
