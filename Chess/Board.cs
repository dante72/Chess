using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Board : IEnumerable<Cell>
    {
        private readonly Cell[,] cells;

        public ChessFigure this[int row, int column]
        {
            get => cells[row, column].ChessFigure;
            set => cells[row, column].ChessFigure = value;
        }

        public Board()
        {
            cells = new Cell[8, 8];
            for (int i = 0; i < cells.GetLength(0); i++)
                for (int j = 0; j < cells.GetLength(1); j++)
                    cells[i, j] = new Cell(i * cells.GetLength(1) + j);
        }

        public IEnumerator<Cell> GetEnumerator()
            => cells.Cast<Cell>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => cells.GetEnumerator();
    }
}
