using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class ViewModel
    {
        public List<int> СhessBoard { set; get; }

        public ViewModel()
        {
            СhessBoard = new List<int>();
            for (int i = 0; i < 64; i++)
                СhessBoard.Add(i + 1);
        }
    }
}
