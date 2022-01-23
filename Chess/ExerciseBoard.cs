using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class ExerciseBoard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public BoardVM Value { get; set; }
        public string Moves { get; set; }
    }
}
