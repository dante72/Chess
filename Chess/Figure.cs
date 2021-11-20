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
        /// <summary>
        /// Доска
        /// </summary>
        public Board Board { get; set; }

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
        public abstract List<Cell> GetPossibleMoves();

        /// <summary>
        /// Король
        /// </summary>
        public class King : Figure
        {
            public King(FigureColors color) : base(color) { }


            public override List<Cell> GetPossibleMoves()
            {
                Cell pos = Board.First(f => f.Figure.Equals(this));
                var list = new List<Cell>();
                for (int i = pos.X - 1; i <= pos.X + 1; i++)
                    for (int j = pos.Y - 1; j <= pos.Y + 1; j++)
                        if (i >= 0 && j >= 0 && i <= 8 && j <= 8 && pos.X != i && pos.Y != j)
                            if (Board[i, j] == null)
                                list.Add(Board.cells[i, j]);
                return list;

            }
        }

        /// <summary>
        /// Ферзь
        /// </summary> 
        public class Queen : Figure
        {
            public Queen(FigureColors color) : base(color) { }
            public override List<Cell> GetPossibleMoves()
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Ладья
        /// </summary>
        public class Rook : Figure
        {
            public Rook(FigureColors color) : base(color) { }
            public override List<Cell> GetPossibleMoves()
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Конь
        /// </summary>
        public class Knight : Figure
        {
            public Knight(FigureColors color) : base(color) { }
            public override List<Cell> GetPossibleMoves()
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Слон
        /// </summary>
        public class Bishop : Figure
        {
            public Bishop(FigureColors color) : base(color) { }
            public override List<Cell> GetPossibleMoves()
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Пешка
        /// </summary>
        public class Pawn : Figure
        {
            public Pawn(FigureColors color) : base(color) { }
            public override List<Cell> GetPossibleMoves()
            {
                Cell pos = Board.First(f => f.Figure == this);
                var list = new List<Cell>();
                for (int i = pos.X - 1; i <= pos.X + 1; i++)
                    for (int j = pos.Y - 1; j <= pos.Y + 1; j++)
                        if (i >= 0 && j >= 0 && i < 8 && j < 8)
                            if (Board.cells[i, j].Figure == null || Board.cells[i, j].Figure.Color != Color)
                                list.Add(Board.cells[i, j]);
                return list;
            }
        }
    }
}
