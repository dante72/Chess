using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    /// <summary>
    /// Фигура, не ходившая за игру
    /// </summary>
    public interface IFirstMove
    {
        int IsFirstMove { get; set; }
    }
}
