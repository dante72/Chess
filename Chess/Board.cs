using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Board
    {
        public List<Cell> Cells { get; set; }

        public Cell this[int row, int column]
        {
            get => Cells[row * 8 + column];
            set
            {
                Cells[row * 8 + column] = value;
            }
        }

        public Board()
        {
            Cells = new List<Cell>();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Cells.Add(new Cell(i, j) { Board = this });
        }
    }
}
