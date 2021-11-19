using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Cell : NotifyPropertyChanged
    {
        public int X { get; }
        public int Y { get; }

        public Figure figure;
        public Figure Figure
        {
            set
            {
                figure = value;
                OnPropertyChanged();
            }
            get => figure;
        }
        public int Index { get => X * 8 + Y; }

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

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
