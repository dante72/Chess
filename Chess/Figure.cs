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
        /// Цвет фигуры
        /// </summary>
        public  FigureColors Color { get; private set; }

        /// <summary>
        /// Позиция фигуры
        /// </summary>
        public Cell Position { get; set; }
        public bool FirstMove { get; set; } = true;

        public Figure(FigureColors color) => Color = color;
        public override string ToString()
        {
            return $"{Color}{GetType().Name}";
        }

        /// <summary>
        /// Получить ячейки в данном направлении (рефакторинг) 
        /// </summary>
        protected List<Cell> GetCellsInDirection(Cell current, Directions direction, int range = 8)
        {
            switch(direction)
            {
                case Directions.Up:
                case Directions.Down:
                    int dir = direction == Directions.Up ? -1 : 1;
                    var list = new List<Cell>();
                    for (int i = 1; i <= range; i++)
                        if (current.Row + i * dir >= 0 && current.Row + i * dir < 8)
                        {
                            if (Position.Board[current.Row + i * dir, current.Column].Figure == null)
                            {
                                list.Add(Position.Board[current.Row + i * dir, current.Column]);
                            }
                            else
                            {
                                if (Position.Board[current.Row + i * dir, current.Column].Figure.Color != Color)
                                    list.Add(Position.Board[current.Row + i * dir, current.Column]);

                                break;
                            }
                        }
                    return list;
                case Directions.Left:
                case Directions.Right:
                    dir = direction == Directions.Left ? -1 : 1;
                    list = new List<Cell>();
                    for (int i = 1; i <= range; i++)
                        if (current.Column + i * dir >= 0 && current.Column + i * dir < 8)
                        {
                            if (Position.Board[current.Row, current.Column + i * dir].Figure == null)
                            {
                                list.Add(Position.Board[current.Row, current.Column + i * dir]);
                            }
                            else
                            {
                                if (Position.Board[current.Row, current.Column + i * dir].Figure.Color != Color)
                                    list.Add(Position.Board[current.Row, current.Column + i * dir]);

                                break;
                            }
                        }
                    return list;
                case Directions.LeftUp:
                case Directions.RightDown:
                    dir = direction == Directions.LeftUp ? -1 : 1;
                    list = new List<Cell>();
                    for (int i = 1; i <= range; i++)
                        if (current.Column + i * dir >= 0 && current.Column + i * dir < 8 && current.Row + i * dir >= 0 && current.Row + i * dir < 8)
                        {
                            if (Position.Board[current.Row + i * dir, current.Column + i * dir].Figure == null)
                            {
                                list.Add(Position.Board[current.Row + i * dir, current.Column + i * dir]);
                            }
                            else
                            {
                                if (Position.Board[current.Row + i * dir, current.Column + i * dir].Figure.Color != Color)
                                    list.Add(Position.Board[current.Row + i * dir, current.Column + i * dir]);

                                break;
                            }
                        }
                    return list;

                case Directions.LeftDown:
                case Directions.RightUp:
                    dir = direction == Directions.LeftDown ? -1 : 1;
                    list = new List<Cell>();
                    for (int i = 1; i <= range; i++)
                        if (current.Column + i * dir >= 0 && current.Column + i * dir < 8 && current.Row - i * dir >= 0 && current.Row - i * dir < 8)
                        {
                            if (Position.Board[current.Row - i * dir, current.Column + i * dir].Figure == null)
                            {
                                list.Add(Position.Board[current.Row - i * dir, current.Column + i * dir]);
                            }
                            else
                            {
                                if (Position.Board[current.Row - i * dir, current.Column + i * dir].Figure.Color != Color)
                                    list.Add(Position.Board[current.Row - i * dir, current.Column + i * dir]);

                                break;
                            }
                        }
                    return list;

                default:
                    throw new NotImplementedException("Error Direction");

            }
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
                Cell pos = Position.Board.Cells.First(f => f.Figure == this);
                var list = new List<Cell>();
                for (int i = pos.Row - 1; i <= pos.Row + 1; i++)
                    for (int j = pos.Column - 1; j <= pos.Column + 1; j++)
                        if (i >= 0 && j >= 0 && i < 8 && j < 8)
                            if (Position.Board[i, j].Figure == null || Position.Board[i, j].Figure.Color != Color)
                                list.Add(Position.Board[i, j]);
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
                Cell position = Position.Board.Cells.First(f => f.Figure == this);
                var list = new List<Cell>();

                list.AddRange(GetCellsInDirection(position, Directions.Down));
                list.AddRange(GetCellsInDirection(position, Directions.Up));
                list.AddRange(GetCellsInDirection(position, Directions.Left));
                list.AddRange(GetCellsInDirection(position, Directions.Right));
                list.AddRange(GetCellsInDirection(position, Directions.LeftUp));
                list.AddRange(GetCellsInDirection(position, Directions.RightDown));
                list.AddRange(GetCellsInDirection(position, Directions.LeftDown));
                list.AddRange(GetCellsInDirection(position, Directions.RightUp));

                return list;
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
                Cell position = Position.Board.Cells.First(f => f.Figure == this);
                var list = new List<Cell>();

                list.AddRange(GetCellsInDirection(position, Directions.Down));
                list.AddRange(GetCellsInDirection(position, Directions.Up));
                list.AddRange(GetCellsInDirection(position, Directions.Left));
                list.AddRange(GetCellsInDirection(position, Directions.Right));

                return list;
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
                Cell position = Position.Board.Cells.First(f => f.Figure == this);
                var list = new List<Cell>();

                for (int i = -2; i <= 2; i++)
                    for (int j = -2; j <= 2; j++)
                    {
                        if (position.Row + i >= 0 && position.Row + i < 8 && position.Column + j >= 0 && position.Column + j < 8)
                            if (i != j && i != -j && i != 0 && j != 0)
                                if (!(Position.Board[position.Row + i, position.Column + j].Figure?.Color == Color))
                                    list.Add(Position.Board[position.Row + i, position.Column + j]);
                    }

                return list;
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
                Cell position = Position.Board.Cells.First(f => f.Figure == this);
                var list = new List<Cell>();

                list.AddRange(GetCellsInDirection(position, Directions.LeftUp));
                list.AddRange(GetCellsInDirection(position, Directions.RightDown));
                list.AddRange(GetCellsInDirection(position, Directions.LeftDown));
                list.AddRange(GetCellsInDirection(position, Directions.RightUp));

                return list;
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
                int range = FirstMove ? 2 : 1;
                var direction = Color == FigureColors.White ? Directions.Up : Directions.Down;
               // Cell position = Position.Board.Cells.First(f => f.Figure == this);

                return GetCellsInDirection(Position, direction, range);
            }
        }
    }
}
