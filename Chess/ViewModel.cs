using System.Collections.ObjectModel;
using System.Linq;

namespace Chess
{
    public class ViewModel : NotifyPropertyChanged
    {
        public BoardVM СhessBoard { set; get; } = new BoardVM();
        public ObservableCollection<CellVM> Cells { set; get; }

        private Figure selectedFigure;

        public Figure SelectedFigure
        {
            set
            {
                selectedFigure = value;

                selectedFigure?.GetCorrectPossibleMoves()
                    .Select(i => СhessBoard[i.Row, i.Column])
                    .ToList()
                    .ForEach(a => a.IsMarked = true);
            }
            get 
            {
                selectedFigure?.GetCorrectPossibleMoves()
                    .Select(i => СhessBoard[i.Row, i.Column])
                    .ToList()
                    .ForEach(a => a.IsMarked = false);

                return selectedFigure;
            }
        }

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

                selectedItem = value;

                if (selectedItem.IsMarked)
                    SelectedFigure.MoveTo(selectedItem.Value);
                else
                if (selectedItem?.Figure != null)
                    SelectedFigure = selectedItem.Figure;
                else
                    SelectedFigure = null;

                selectedItem.IsSelected = true;

            }
            get
            {
                return selectedItem;
            }
        
        }
    }
}
