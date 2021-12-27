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
        public IList<Cell> PossibleMoves { 
            get => GetPossibleMoves();
        }

        public virtual float Weight
        {
           get { return Color == FigureColors.White ? 1.0f : -1.0f * Value; }
        }

        /// <summary>
        /// Ценность фигуры
        /// </summary>
        protected virtual int Value { get; set; } = 0;
        /// <summary>
        /// Цвет фигуры
        /// </summary>
        public  FigureColors Color { get; private set; }

        /// <summary>
        /// Позиция фигуры
        /// </summary>
        public Cell Position { set; get; }

        private Board Board { get => Position?.Board; }
        public int IsFirstMove { get; set; }

        public Figure(FigureColors color, int firstMove = 0)
        {
            Color = color;
            IsFirstMove = firstMove;
        }
        public override string ToString()
        {
            return $"{Color}{GetType().Name}";
        }

        public virtual void MoveTo(Cell to)
        {

            Position.Figure = null;
            to.Figure = this;
            if (this is Pawn && (to.Row == 0 || to.Row == 7))
                to.Figure = new Queen(Color);
            Board.Index++;
            IsFirstMove++;
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
        public bool IsMove()
        {
            return Color == FigureColors.Black && Board.Index % 2 != 0 || Color == FigureColors.White && Board.Index % 2 == 0;
        }

        /// <summary>
        /// Возможные ходы до первого препядствия (другой фигуры)
        /// </summary>
        public abstract List<Cell> GetAllPossibleMoves();

        /// <summary>
        /// Возможные ходы с заходом на клетки противника
        /// </summary>
        /// 
        public virtual List<Cell> GetCorrectPossibleMoves()
        {
            if (IsMove()) return new List<Cell>();

            var moves = GetAllPossibleMoves().Where(i => i.Figure?.Color != Color).ToList();

            return moves;
        }

        public virtual List<Cell> GetPossibleMoves()
        {
            return GetCorrectPossibleMoves().Where(i => !Сheckmate(this, i)).ToList();
        }
        public abstract Figure Clone();

        protected bool Сheckmate(Figure figure, Cell pos)
        {
            var board = new Board(Board, figure.Position, pos);
            return board.KingСheck(figure.Color);
        }

        /// <summary>
        /// Король
        /// </summary>
        public class King : Figure
        {
            public override float Weight
            {
                get
                {
                    return Color == FigureColors.White ? Value + EvaluateBoard.KingEvalWhite[Position.Row, Position.Column] :
                        -Value - EvaluateBoard.KingEvalBlack[Position.Row, Position.Column];
                }
            }
            protected override int Value { get; set; } = 900;
            public King(FigureColors color, int firstMove = 0) : base(color, firstMove) { }
            public override Figure Clone() => new King(Color, IsFirstMove);

            public override void MoveTo(Cell to)
            {
                if (IsFirstMove == 0 && (Position.Column == 3 || Position.Column == 4))
                    Сastling(to);

                base.MoveTo(to);
            }
            /// <summary>
            /// Рокировка
            /// </summary>
            /// <param name="cell">Выбранная ячейка для хода</param>
            private void Сastling(Cell cell)
            {
                if (Position.Column - cell.Column == -2)
                {
                    var cells = GetCellsInDirection(Position, Directions.Right);
                    var rook =  cells.First(i => i.Figure is Rook).Figure;

                    rook.Position.Figure = null;
                    Board[Position.Row, Position.Column + 1].Figure = rook;
                    

                }
                else
                if (Position.Column - cell.Column == 2)
                {
                    var cells = GetCellsInDirection(Position, Directions.Left);
                    var rook = cells.First(i => i.Figure is Rook).Figure;

                    rook.Position.Figure = null;
                    Board[Position.Row, Position.Column - 1].Figure = rook;
                }
            }

            public List<Cell> KingPossibleMoves()
            {
                var list = new List<Cell>();
                for (int i = Position.Row - 1; i <= Position.Row + 1; i++)
                    for (int j = Position.Column - 1; j <= Position.Column + 1; j++)
                        if (i >= 0 && j >= 0 && i < 8 && j < 8 && !(i == Position.Row && j == Position.Column))
                            list.Add(Board[i, j]);
                
                return list;
            }
            public override List<Cell> GetAllPossibleMoves()
            {
                var list = new List<Cell>();
                for (int i = Position.Row - 1; i <= Position.Row + 1; i++)
                    for (int j = Position.Column - 1; j <= Position.Column + 1; j++)
                        if (i >= 0 && j >= 0 && i < 8 && j < 8 && !(i == Position.Row && j == Position.Column))
                            list.Add(Board[i, j]);

                if (IsFirstMove == 0 && !Board.KingСheck(Color))
                {
                    var left = GetCellsInDirection(Position, Directions.Left);
                    var right = GetCellsInDirection(Position, Directions.Right);
                    if (left.Count != 0)
                    if (left.All(i => i.Figure == null || i.Figure.IsFirstMove == 0 && i.Figure is Rook) && left.Last().Figure != null)
                        list.Add(Board[Position.Row, Position.Column - 2]);
                    if (right.Count != 0)
                    if (right.All(i => i.Figure == null || i.Figure.IsFirstMove == 0 && i.Figure is Rook) && right.Last().Figure != null)
                        list.Add(Board[Position.Row, Position.Column + 2]);
                }

                return list.Where(i => !Сheckmate(this, i)).ToList();
            }

            public override List<Cell> GetPossibleMoves()
            {
                return GetCorrectPossibleMoves().Where(i => !Сheckmate(this, i)).ToList();
            }

            public override List<Cell> GetCorrectPossibleMoves()
            {
                if (IsMove()) return new List<Cell>();

                var moves = GetAllPossibleMoves().Where(i => i.Figure?.Color != Color).ToList();

                if (IsFirstMove == 0)
                {
                    if (moves.Contains(Board[Position.Row, Position.Column - 2]) && !moves.Contains(Board[Position.Row, Position.Column - 1]))
                        moves.Remove(Board[Position.Row, Position.Column - 2]);
                    if (moves.Contains(Board[Position.Row, Position.Column + 2]) && !moves.Contains(Board[Position.Row, Position.Column + 1]))
                        moves.Remove(Board[Position.Row, Position.Column + 2]);
                }

                return moves;
            }
        }

        /// <summary>
        /// Ферзь
        /// </summary> 
        public class Queen : Figure
        {
            public override float Weight
            {
                get
                {
                    return Color == FigureColors.White ? Value + EvaluateBoard.QueenEval[Position.Row, Position.Column] :
                        -Value - EvaluateBoard.QueenEval[Position.Row, Position.Column];
                }
            }
            protected override int Value { get; set; } = 90;
            public Queen(FigureColors color) : base(color) { }
            public override Figure Clone() => new Queen(Color);
            public override List<Cell> GetAllPossibleMoves()
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
            public override float Weight
            {
                get
                {
                    return Color == FigureColors.White ? Value + EvaluateBoard.RookEvalWhite[Position.Row, Position.Column] :
                        -Value - EvaluateBoard.RookEvalBlack[Position.Row, Position.Column];
                }
            }
            protected override int Value { get; set; } = 50;
            public Rook(FigureColors color, int firstMove = 0) : base(color, firstMove) { }
        public override Figure Clone() => new Rook(Color, IsFirstMove);
            public override List<Cell> GetAllPossibleMoves()
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
            public override float Weight
            {
                get
                {
                    return Color == FigureColors.White ? Value + EvaluateBoard.KnightEval[Position.Row, Position.Column] :
                        -Value - EvaluateBoard.KnightEval[Position.Row, Position.Column];
                }
            }
            protected override int Value { get; set; } = 30;
            public Knight(FigureColors color) : base(color) { }
            public override Figure Clone() => new Knight(Color);
            public override List<Cell> GetAllPossibleMoves()
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
            public override float Weight
            {
                get
                {
                    return Color == FigureColors.White ? Value + EvaluateBoard.BishopEvalWhite[Position.Row, Position.Column] :
                        -Value - EvaluateBoard.BishopEvalBlack[Position.Row, Position.Column];
                }
            }
            protected override int Value { get; set; } = 30;
            public Bishop(FigureColors color) : base(color) { }
            public override Figure Clone() => new Bishop(Color);
            public override List<Cell> GetAllPossibleMoves()
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
            public override float Weight
            {
                get
                {
                    return Color == FigureColors.White ? Value + EvaluateBoard.PawnEvalWhite[Position.Row, Position.Column] :
                        -Value - EvaluateBoard.PawnEvalBlack[Position.Row, Position.Column];
                }
            }
            protected override int Value { get; set; } = 10;

            private int count = -2;

            private Pawn pawn;
            public Pawn(FigureColors color, int firstMove = 0) : base(color, firstMove) { }
            public override Figure Clone() => new Pawn(Color, IsFirstMove);
            public override void MoveTo(Cell to)
            {
                if (pawn != null && to.Figure == null && to.Column - Position.Column != 0)
                {
                    pawn.Position.Figure = null;
                }
                base.MoveTo(to);

                if (IsFirstMove == 1 && (Position.Row == 3 || Position.Row == 4))
                {
                    if (Position[0, 1]?.Figure is Pawn p1 && p1.Color != Color)
                    {
                        p1.count = Board.Index;
                        p1.pawn = this;
                    }
                    if (Position[0, -1]?.Figure is Pawn p2 && p2.Color != Color)
                    {
                        p2.count = Board.Index;
                        p2.pawn = this;
                    }
                }               
            }
            public override List<Cell> GetCorrectPossibleMoves()
            {
                if (IsMove()) return new List<Cell>();

                int range = IsFirstMove == 0 && (Position.Row == 1 || Position.Row == 6)? 2 : 1;
                var direction = Color == FigureColors.White ? Directions.Up : Directions.Down;
                var fields = GetAllPossibleMoves()
                    .Where(i => i.Figure != null && i.Figure?.Color != Color || count == Board.Index && (Board[i.Row + 1, i.Column].Figure is Pawn p1  && p1 == pawn || Board[i.Row - 1, i.Column].Figure is Pawn p2 && p2 == pawn))
                    .ToList();
                fields.AddRange(GetCellsInDirection(Position, direction, range).Where(i => i.Figure == null));

                if (Сheckmate(this, Position))
                    fields = fields.Where(i => !Сheckmate(this, i)).ToList();

                return fields;
            }
            public override List<Cell> GetAllPossibleMoves()
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
