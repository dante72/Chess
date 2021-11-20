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
        public readonly Cell[,] cells;

        public Figure this[int row, int column]
        {
            get => cells[row, column]?.Figure;
            set
            {
                value.Board = this;
                cells[row, column].Figure = value;  
            }
        }

        public Board()
        {
            cells = new Cell[8, 8];
            for (int i = 0; i < cells.GetLength(0); i++)
                for (int j = 0; j < cells.GetLength(1); j++)
                    cells[i, j] = new Cell(i, j);
        }

        public IEnumerator<Cell> GetEnumerator()
            => cells.Cast<Cell>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => cells.GetEnumerator();
    }
}
