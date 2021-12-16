﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.Figure;

namespace Chess
{
    public class Cell : NotifyPropertyChanged
    {
        public int Row { get; }
        public int Column { get; }

        public Board Board { get; }

        private Figure figure;
        public Figure Figure {
            get => figure;
            set
            { 
                figure = value;

                if (figure != null)
                   figure.Position = this;

                OnPropertyChanged();
            }
        }

        public Cell this[int i, int j]
        {
            get
            {
                if (Row + i >= 0 && Row + i < 8 && Column + j >= 0 && Column + j < 8)
                    return Board[Row + i, Column + j];
                else
                    return null;
            }
        }
        public Cell(int row, int column, Board board = null)
        {
            Row = row;
            Column = column;
            Board = board;
        }

        public static bool operator ==(Cell b1, Cell b2)
        {
            return b1.Column == b2.Column && b1.Row == b2.Row;
        }

        public static bool operator !=(Cell b1, Cell b2)
        {
            return !(b1 == b2);
        }

        public override string ToString()
        {
            return $"{(char)('a' + Column)}{Row + 1}";
        }
    }
}
