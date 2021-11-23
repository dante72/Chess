using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.Figure;

namespace Chess
{
    public class Board
    {
        public List<Cell> Cells { get; private set; }
        public Cell this[int row, int column]
        {
            get => Cells[row * 8 + column];
            private set
            {
                Cells[row * 8 + column] = value;
            }
        }

        public Board()
        {
            Cells = new List<Cell>();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Cells.Add(new Cell(i, j, this));
            SetupСhessBoard();
        }
        private void SetupСhessBoard()
        {   
            this[0, 0].Figure = new Rook(FigureColors.Black);
            this[0, 1].Figure = new Knight(FigureColors.Black);
            this[0, 2].Figure = new Bishop(FigureColors.Black);
            this[0, 3].Figure = new Queen(FigureColors.Black);
            this[0, 4].Figure = new King(FigureColors.Black);
            this[0, 5].Figure = new Bishop(FigureColors.Black);
            this[0, 6].Figure = new Knight(FigureColors.Black);
            this[0, 7].Figure = new Rook(FigureColors.Black);
            for (int i = 0; i < 8; i++)
            {
                this[1, i].Figure = new Pawn(FigureColors.Black);
                this[6, i].Figure = new Pawn(FigureColors.White);
            }
            this[7, 0].Figure = new Rook(FigureColors.White);
            this[7, 1].Figure = new Knight(FigureColors.White);
            this[7, 2].Figure = new Bishop(FigureColors.White);
            this[7, 3].Figure = new Queen(FigureColors.White);
            this[7, 4].Figure = new King(FigureColors.White);
            this[7, 5].Figure = new Bishop(FigureColors.White);
            this[7, 6].Figure = new Knight(FigureColors.White);
            this[7, 7].Figure = new Rook(FigureColors.White);
        }

    }
}
