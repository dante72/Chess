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

        private Board Board { get => Position?.Board; }
        public bool FirstMove { get; set; } = true;

        public Figure(FigureColors color)
        {
            Color = color;
        }
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
                            list.Add(Board[current.Row + i * dir, current.Column]);
                            if (Board[current.Row + i * dir, current.Column].Figure != null)
                                break;
                        }
                    return list;
                case Directions.Left:
                case Directions.Right:
                    dir = direction == Directions.Left ? -1 : 1;
                    list = new List<Cell>();
                    for (int i = 1; i <= range; i++)
                        if (current.Column + i * dir >= 0 && current.Column + i * dir < 8)
                        {
                            list.Add(Board[current.Row, current.Column + i * dir]);
                            if (Board[current.Row, current.Column + i * dir].Figure != null)
                                break;
                        }
                    return list;
                case Directions.LeftUp:
                case Directions.RightDown:
                    dir = direction == Directions.LeftUp ? -1 : 1;
                    list = new List<Cell>();
                    for (int i = 1; i <= range; i++)
                        if (current.Column + i * dir >= 0 && current.Column + i * dir < 8 && current.Row + i * dir >= 0 && current.Row + i * dir < 8)
                        {
                            list.Add(Board[current.Row + i * dir, current.Column + i * dir]);
                            if (Board[current.Row + i * dir, current.Column + i * dir].Figure != null)
                                break;
                        }
                    return list;

                case Directions.LeftDown:
                case Directions.RightUp:
                    dir = direction == Directions.LeftDown ? -1 : 1;
                    list = new List<Cell>();
                    for (int i = 1; i <= range; i++)
                        if (current.Column + i * dir >= 0 && current.Column + i * dir < 8 && current.Row - i * dir >= 0 && current.Row - i * dir < 8)
                        {
                            list.Add(Board[current.Row - i * dir, current.Column + i * dir]);
                            if (Board[current.Row - i * dir, current.Column + i * dir].Figure != null)
                                break;
                        }
                    return list;

                default:
                    throw new NotImplementedException("Error Direction");

            }
        }

        /// <summary>
        /// Возможные ходы до первого препядствия (другой фигуры)
        /// </summary>
        public abstract List<Cell> GetPossibleMoves(bool recursionOfKings = false);

        /// <summary>
        /// Возможные ходы только с фигурами противника
        /// </summary>
        public virtual List<Cell> GetPossibleMovesWithEnemyOnly()
        {
            return GetPossibleMoves().Where(i => i.Figure?.Color != Color).ToList();
        }

        public virtual List<Cell> GetPossibleMovesWithCheckOfKing()
        {
            return GetPossibleMoves().Where(i => i.Figure?.Color != Color).ToList();
        }

        /// <summary>
        /// Король
        /// </summary>
        public class King : Figure
        {
            public King(FigureColors color) : base(color) { }
            public override List<Cell> GetPossibleMoves(bool recursionOfKings = false)
            {

                var list = new List<Cell>();
                for (int i = Position.Row - 1; i <= Position.Row + 1; i++)
                    for (int j = Position.Column - 1; j <= Position.Column + 1; j++)
                        if (i >= 0 && j >= 0 && i < 8 && j < 8 && !(i == Position.Row && j == Position.Column))
                            list.Add(Board[i, j]);
                if (!recursionOfKings)
                {
                    var figures = Board.Cells.Where(i => i.Figure != null && i.Figure.Color != Color)
                        .Select(i => i.Figure.GetPossibleMoves(true));
                    list = list.Where(i => !figures.Any(j => j.Contains(i))).ToList();
                }

                return list;

            }
        }

        /// <summary>
        /// Ферзь
        /// </summary> 
        public class Queen : Figure
        {
            public Queen(FigureColors color) : base(color) { }
            public override List<Cell> GetPossibleMoves(bool recursionOfKings = false)
            {
                var list = new List<Cell>();

                list.AddRange(GetCellsInDirection(Position, Directions.Down));
                list.AddRange(GetCellsInDirection(Position, Directions.Up));
                list.AddRange(GetCellsInDirection(Position, Directions.Left));
                list.AddRange(GetCellsInDirection(Position, Directions.Right));
                list.AddRange(GetCellsInDirection(Position, Directions.LeftUp));
                list.AddRange(GetCellsInDirection(Position, Directions.RightDown));
                list.AddRange(GetCellsInDirection(Position, Directions.LeftDown));
                list.AddRange(GetCellsInDirection(Position, Directions.RightUp));

                return list;
            }
        }

        /// <summary>
        /// Ладья
        /// </summary>
        public class Rook : Figure
        {
            public Rook(FigureColors color) : base(color) { }
            public override List<Cell> GetPossibleMoves(bool recursionOfKings = false)
            {
                var list = new List<Cell>();

                list.AddRange(GetCellsInDirection(Position, Directions.Down));
                list.AddRange(GetCellsInDirection(Position, Directions.Up));
                list.AddRange(GetCellsInDirection(Position, Directions.Left));
                list.AddRange(GetCellsInDirection(Position, Directions.Right));

                return list;
            }
        }

        /// <summary>
        /// Конь
        /// </summary>
        public class Knight : Figure
        {
            public Knight(FigureColors color) : base(color) { }
            public override List<Cell> GetPossibleMoves(bool recursionOfKings = false)
            {
                var list = new List<Cell>();

                for (int i = -2; i <= 2; i++)
                    for (int j = -2; j <= 2; j++)
                    {
                        if (Position.Row + i >= 0 && Position.Row + i < 8 && Position.Column + j >= 0 && Position.Column + j < 8)
                            if (i != j && i != -j && i != 0 && j != 0)
                                list.Add(Board[Position.Row + i, Position.Column + j]);
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
            public override List<Cell> GetPossibleMoves(bool recursionOfKings = false)
            {
                var list = new List<Cell>();

                list.AddRange(GetCellsInDirection(Position, Directions.LeftUp));
                list.AddRange(GetCellsInDirection(Position, Directions.RightDown));
                list.AddRange(GetCellsInDirection(Position, Directions.LeftDown));
                list.AddRange(GetCellsInDirection(Position, Directions.RightUp));

                return list;
            }
        }

        /// <summary>
        /// Пешка
        /// </summary>
        public class Pawn : Figure
        {
            public override List<Cell> GetPossibleMovesWithEnemyOnly()
            {   
                int range = FirstMove ? 2 : 1;
                var direction = Color == FigureColors.White ? Directions.Up : Directions.Down;
                var fields = GetPossibleMoves()
                    .Where(i => i.Figure != null && i.Figure?.Color != Color)
                    .ToList();
                fields.AddRange(GetCellsInDirection(Position, direction, range).Where(i => i.Figure == null));

                return fields;
            }
            public Pawn(FigureColors color) : base(color) { }
            public override List<Cell> GetPossibleMoves(bool recursionOfKings = false)
            {
                var direction = Color == FigureColors.White ? Directions.Up : Directions.Down;

                List<Cell> attackFields = new List<Cell>();

                if (direction == Directions.Up)
                {
                    attackFields.AddRange(GetCellsInDirection(Position, Directions.LeftUp, 1));
                    attackFields.AddRange(GetCellsInDirection(Position, Directions.RightUp, 1));
                }
                else
                {
                    attackFields.AddRange(GetCellsInDirection(Position, Directions.LeftDown, 1));
                    attackFields.AddRange(GetCellsInDirection(Position, Directions.RightDown, 1));
                }

                return attackFields;
            }
        }
    }
}
