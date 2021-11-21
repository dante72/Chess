using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Cell
    {
        public int Row { get; }
        public int Column { get; }

        private Figure figure;
        public Figure Figure {
            get => figure;
            set
            {
                figure = value;
                if (figure != null)
                    figure.Position = this;
            }
        
        }
        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
