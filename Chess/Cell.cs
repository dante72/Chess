using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Cell : NotifyPropertyChanged
    {
        public ChessFigure ChessFigure { set; get; }
        public int Index { set; get; }

        private bool isSelected;
        public bool IsSelected
        {
            set {
                isSelected = value;
                OnPropertyChanged();
            }
            get => isSelected;
        }

        private bool isMouseOver;
        public bool IsMouseOver
        {
            set {
                isMouseOver = value;
                OnPropertyChanged();
            }
            get => isMouseOver;
        }

        public Cell(int index = 0, ChessFigure figure = ChessFigure.Empty)
        {
            Index = index;
            ChessFigure = figure;
        }
    }
}
