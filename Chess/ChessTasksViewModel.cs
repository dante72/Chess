using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class ChessTasksViewModel
    {
        public List<BoardVM> Boards { get { return _Boards.Select(board => new BoardVM(board)).ToList(); } }
        public List<Board> _Boards { set; get; }

        public BoardVM SelectedItem { set; get; }
        public ChessTasksViewModel()
        {
            _Boards = new List<Board>();
            _Boards.Add(new Board());
            _Boards.Add(new Board());
        }

        public Board SelectedBoard { set; get; }

        /*private RelayCommand okCommand;
        public RelayCommand OkCommand
        {
            get
            {
                return okCommand ??
                    (okCommand = new RelayCommand(obj =>
                    {
                        if (SelectedItem != null)
                        {
                            SelectedBoard = SelectedItem.board;
                            //Close();
                        }
                    }));
            }
        }

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return cancelCommand ??
                    (cancelCommand = new RelayCommand(obj =>
                    {
                        var dialog = new ChessTasks();
                        dialog.ShowDialog();
                    }));
            }
        }*/
    }
}
