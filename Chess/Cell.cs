using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Cell : NotifyPropertyChanged
    {
        public int Row { get; }
        public int Column { get; }

        public Figure figure;
        public Figure Figure
        {
            set
            {
                if (figure != null)
                    figure.FirstMove = false;
                figure = value;
                OnPropertyChanged();
            }
            get => figure;
        }
        public int Index { get => Row * 8 + Column; }

        private bool isSelected;
        public bool IsSelected
        {
            set {
                isSelected = value;
                OnPropertyChanged();
            }
            get => isSelected;
        }

        private bool isMarked;
        public bool IsMarked
        {
            set {
                isMarked = value;
                OnPropertyChanged();
            }
            get => isMarked;
        }

        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
