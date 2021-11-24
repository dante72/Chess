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
                if (figure != null && figure.FirstMove && figure.GetType() == typeof(King) && Math.Abs(figure.Position.Column - Column) == 2)
                    (figure as King).Сastling(this);
                if (figure != null)
                    figure.FirstMove = false;
                figure = value;

                if (figure != null)
                    figure.Position = this;


                OnPropertyChanged();
            }
        }
        public Cell(int row, int column, Board board = null)
        {
            Row = row;
            Column = column;
            Board = board;
        }
    }
}
