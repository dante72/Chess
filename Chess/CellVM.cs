using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class CellVM : NotifyPropertyChanged
    {
        private Cell Value { set; get; }
        public Figure Figure
        {
            set
            {
                if (Value.Figure != null)
                    Value.Figure.FirstMove = false;
                Value.Figure = value;

                OnPropertyChanged();
            }
            get => Value.Figure;
        }
        public int Index { get => Value.Row * 8 + Value.Column; }

        private bool isSelected;
        public bool IsSelected
        {
            set
            {
                isSelected = value;

                OnPropertyChanged();
            }
            get => isSelected;
        }

        private bool isMarked;
        public bool IsMarked
        {
            set
            {
                isMarked = value;

                OnPropertyChanged();
            }
            get => isMarked;
        }

        public CellVM(Cell cell) => Value = cell;
    }
}
