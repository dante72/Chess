using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.DB;

namespace Chess
{
    public class ChessTasksViewModel
    {
        public List<BoardVM> Boards { get { return _Boards.Select(board => new BoardVM(board)).ToList(); } }
        private List<Board> _Boards { set; get; }

        public BoardVM SelectedItem { set; get; }
        public ChessTasksViewModel()
        {
            //_Boards = new List<Board>();
            //_Boards.Add(new Board(@"b3WB b5WN b6WK d5WN e5BP e6BK d7BP f7BP h7WQ"));
            //_Boards.Add(new Board("a3WP a4BP c5BK c6BP d8WB d2WR e2WQ e4WK f4WP"));
            //_Boards.Add(new Board("a7WB d2WR g7WN g2BQ g1WN h1BB h2BK h4BP g7WR h8WK"));
            //_Boards.Add(new Board("b6WK e5BK h6WQ c2WB"));
            //_Boards.Add(new Board("a4BK f8WB g5WQ e1WK"));

            using (var context = new InfoContext())
            {
                /*var ex = new Exercise()
                {
                    Name = "",
                    Value = "a3WP a4BP c5BK c6BP d8WB d2WR e2WQ e4WK f4WP",
                    Moves = 2
                };*/
                //context.Exercises.Add(ex);
                //context.SaveChanges();
                _Boards = context.Exercises.ToList().Select(i => new Board(i.Value)).ToList();
            }
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
