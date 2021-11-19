using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    /// <summary>
    /// Базовый класс для фигур
    /// </summary>
    public abstract class Figure : IFirstMove
    {
        public static Figure BlackKing { private set; get; } = new King(FigureColors.Black);
        public static Figure BlackQueen { private set; get; } = new Queen(FigureColors.Black);
        public static Figure BlackRook { private set; get; } = new Rook(FigureColors.Black);
        public static Figure BlackKnight { private set; get; } = new Knight(FigureColors.Black);
        public static Figure BlackBishop { private set; get; } = new Bishop(FigureColors.Black);
        public static Figure BlackPawn { private set; get; } = new Pawn(FigureColors.Black);
        public static Figure WhiteKing { private set; get; } = new King(FigureColors.White);
        public static Figure WhiteQueen { private set; get; } = new Queen(FigureColors.White);
        public static Figure WhiteRook { private set; get; } = new Rook(FigureColors.White);
        public static Figure WhiteKnight { private set; get; } = new Knight(FigureColors.White);
        public static Figure WhiteBishop { private set; get; } = new Bishop(FigureColors.White);
        public static Figure WhitePawn { private set; get; } = new Pawn(FigureColors.White);
        public Cell Position { get; set; }
        /// <summary>
        /// Цвет фигуры
        /// </summary>
        public  FigureColors Color { get; private set; }
        public bool FirstMove { get; set; } = false;

        public Figure(FigureColors color) => Color = color;
        public override string ToString()
        {
            return $"{Color}{GetType().Name}";
        }

        /// <summary>
        /// Возможные ходы
        /// </summary>
        public abstract IEnumerable<Cell> GetPossibleMoves(Board cells);

        /// <summary>
        /// Король
        /// </summary>
        class King : Figure
        {
            public King(FigureColors color) : base(color) { }


            public override IEnumerable<Cell> GetPossibleMoves(Board cells)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Ферзь
        /// </summary> 
        class Queen : Figure
        {
            public Queen(FigureColors color) : base(color) { }
            public override IEnumerable<Cell> GetPossibleMoves(Board cells)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Ладья
        /// </summary>
        class Rook : Figure
        {
            public Rook(FigureColors color) : base(color) { }
            public override IEnumerable<Cell> GetPossibleMoves(Board cells)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Конь
        /// </summary>
        class Knight : Figure
        {
            public Knight(FigureColors color) : base(color) { }
            public override IEnumerable<Cell> GetPossibleMoves(Board cells)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Слон
        /// </summary>
        class Bishop : Figure
        {
            public Bishop(FigureColors color) : base(color) { }
            public override IEnumerable<Cell> GetPossibleMoves(Board cells)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Пешка
        /// </summary>
        class Pawn : Figure
        {
            public Pawn(FigureColors color) : base(color) { }
            public override IEnumerable<Cell> GetPossibleMoves(Board cells)
            {
                throw new NotImplementedException();
            }
        }
    }
}
