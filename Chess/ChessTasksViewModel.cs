using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class ChessTasksViewModel
    {
        public List<BoardVM> Boards { set; get; }


        public ChessTasksViewModel()
        {
            Boards = new List<BoardVM>();
            Boards.Add(new BoardVM());
            Boards.Add(new BoardVM());
        }

    }
}
