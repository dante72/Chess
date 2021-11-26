using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.Figure;

namespace Chess
{
    public class ViewModel : NotifyPropertyChanged
    {
        public BoardVM СhessBoard { set; get; } = new BoardVM();
        public ObservableCollection<CellVM> Cells { set; get; }

        private CellVM selectedItem;
        
        /// <summary>
        /// Выбранная клетка на доске (вся локика доски)
        /// </summary>
        public CellVM SelectedItem
        {
            set
            {
                //снять выбор с предыдущей фигуры
                if (selectedItem != null)
                    selectedItem.IsSelected = false;

                //если выбрана не отмеченная ячейка, сбросить все отмеченные
                if (value.IsMarked == false)
                    selectedItem?.Value.Figure?.GetPossibleMovesWithEnemyOnly()
                        .Select(i => СhessBoard[i.Row, i.Column])
                        .ToList()
                        .ForEach(a => a.IsMarked = false);

                //если выбрана отмеченная ячейка, сбросить возможные ходы
                if (value.IsMarked == true)
                { 
                    selectedItem?.Value.Figure?.GetPossibleMovesWithEnemyOnly()
                        .Select(i => СhessBoard[i.Row, i.Column])
                        .ToList()
                        .ForEach(a => a.IsMarked = false);

                    selectedItem.Value.Figure?.MoveTo(value.Value);
                }
                else
                {
                    // если не выбрана отмеченная ячейка с фигурой, отметить возможные ходы
                     selectedItem = value;
                    selectedItem.Value.Figure?.GetPossibleMovesWithEnemyOnly()
                        .Select(i => СhessBoard[i.Row, i.Column])
                        .ToList()
                        .ForEach(a => a.IsMarked = true);

                    //отметить ячейку
                    selectedItem.IsSelected = true;
                }
            }
            get => selectedItem;
        
        }

        public ViewModel()
        {
        }
    }
}
