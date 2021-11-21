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
                if (selectedItem != null)
                    selectedItem.IsSelected = false;
                
                if (value.IsMarked == false)
                    selectedItem?.Figure?.GetPossibleMoves().Select(i => СhessBoard[i.Row, i.Column]).ToList()
                        .ForEach(a => a.IsMarked = false);

                if (value?.IsMarked == true)
                { 
                    selectedItem?.Figure?.GetPossibleMoves().Select(i => СhessBoard[i.Row, i.Column]).ToList()
                        .ForEach(a => a.IsMarked = false);
                    value.Figure = selectedItem.Figure;
                    selectedItem.Figure = null;

                }
                else
                {
                    selectedItem = value;
                    selectedItem?.Figure?.GetPossibleMoves().Select(i => СhessBoard[i.Row, i.Column]).ToList()
                    .ForEach(a => a.IsMarked = true);

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
