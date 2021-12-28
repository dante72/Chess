using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    /// <summary>
    /// Ячейка для ViewModel c доп. свойствами
    /// </summary>
    public class CellVM : NotifyPropertyChanged
    {
        /// <summary>
        /// Ссылка на ячейку
        /// </summary>
        public Cell Value { set; get; }
        public Figure Figure
        {
            set
            {
                Value.Figure = value;
            }
            get => Value.Figure;
        }
        public int Index { get => Value.Row * 8 + Value.Column; }

        private bool isSelected;

        /// <summary>
        /// Помечает выбранную пользователем ячейку
        /// </summary>
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

        /// <summary>
        /// Отмечает возможные клетки для хода
        /// </summary>
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
