using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Chess.Figure;

namespace Chess
{
    public class Board
    {
        public Stack<Figure> MovingFigures { set; get; } = new Stack<Figure>();
        public bool IsCheckMate { get => isCheckMate(); }
        public int Index { set; get; }

        public int Moves { set; get; }
        public List<Cell> Cells { get; private set; }
        public Cell this[int row, int column]
        {
            get
            {
                return Cells[row * 8 + column];
            }
            private set
            {
                Cells[row * 8 + column] = value;
            }
        }
        public void MoveBack()
        {
            if (MovingFigures.Count == 0)
                return;
            var figure = MovingFigures.Pop();
            Back(figure);
            if (MovingFigures.Count > 0)
            {
                var figure2 = MovingFigures.Peek();
                if (figure.boardIndex == figure2.boardIndex)
                {
                    Back(MovingFigures.Pop());
                }
            }
            Index--;
        }

        private void Back(Figure figure)
        {
            figure.Position.Figure = null;
            var cell = figure.Moves.Pop();
            cell.Figure = figure;
            figure.IsFirstMove--;
        }
        /// <summary>
        /// Создать доску с начальной расстановкой фигур
        /// </summary>

        public Board(string info = @"a2WP b2WP c2WP d2WP e2WP f2WP g2WP h2WP
                                     a1WR b1WN c1WB d1WQ e1WK f1WB g1WN h1WR
                                     a7BP b7BP c7BP d7BP e7BP f7BP g7BP h7BP
                                     a8BR b8BN c8BB d8BK e8BQ f8BB g8BN h8BR", int moves = -1)
        {
            Moves = moves * 2 - 1;
            Index = 1;
            Cells = new List<Cell>();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Cells.Add(new Cell(i, j, this));


            int k = 0;
            while(k < info.Length)
            {
                while (info[k] == ' ' || info[k] == '\t' || info[k] == '\r' || info[k] == '\n')
                    k++;
                int column = info[k++] - 'a';
                int row = 8 - (info[k++] - '0');
                var color = info[k++] == 'W' ? FigureColors.White : FigureColors.Black;
                var figure = GetFigure(info[k++], color);

                this[row, column].Figure = figure;
                if (figure is King && (row == 7 && column == 3 || row == 0 && column == 4))
                    figure.IsFirstMove = 0;

            }
            //SetupСhessBoard7();
        }

        Figure GetFigure(char f, FigureColors color)
        {
            switch (f)
            {
                case 'K': return new King(color);
                case 'Q': return new Queen(color);
                case 'B': return new Bishop(color);
                case 'N': return new Knight(color);
                case 'R': return new Rook(color);
                case 'P': return new Pawn(color);
                default: throw new Exception("Error figure");
            }
        }

        /// <summary>
        /// Создать копию доски с перемещением одной фигуры (from, to равны = фигура убрана)
        /// </summary>
        public Board(Board copy, Cell from, Cell to)
        {
            Moves = copy.Moves;
            Index = copy.Index;
            Cells = new List<Cell>();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    var cell = new Cell(i, j, this);
                    var figure = copy[i, j].Figure?.Clone();
                    if (figure != null) { cell.Figure = figure; }
                    Cells.Add(cell);
                }

            this[from.Row, from.Column].Figure.MoveTo(this[to.Row, to.Column]);
        }

        public Board(Board copy)
        {
            Moves = copy.Moves;
            Index = copy.Index;
            Cells = new List<Cell>();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    var cell = new Cell(i, j, this);
                    var figure = copy[i, j].Figure?.Clone();
                    if (figure != null) { cell.Figure = figure; }
                    Cells.Add(cell);
                }
        }

        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    str += this[i, j].Figure == null ? " . " : $" {this[i, j].Figure.GetType().Name.Substring(0, 1)} ";
                }
                str += "\n";
            }
            return str;
        }

        /// <summary>
        /// Проверка на ШАХ
        /// </summary>
        public bool KingСheck(FigureColors color)
        {

                var king = Cells.FirstOrDefault(i => i.Figure is King k && k.Color == color);
                King enemyKing = (King)Cells.FirstOrDefault(i => i.Figure is King k && k.Color != color)?.Figure;
                return Cells
                    .Where(i => i.Figure != null && i.Figure.Color != color && i.Figure.GetType() != typeof(King))
                    .Select(i => i.Figure.GetAllPossibleMoves())
                    .Any(i => i.Contains(king)) || enemyKing != null && enemyKing.KingPossibleMoves().Contains(king);

        }

        public bool isCheckMate()
        {
            var color = Index % 2 != 0 ? FigureColors.White : FigureColors.Black;
            var figurs = Cells.Where(i => i.Figure != null && i.Figure.Color == color).Select(i => i.Figure).ToList();
            return !figurs.SelectMany(i => i.PossibleMoves).Any() && KingСheck(color);
        }

        public bool isCheckPate()
        {
            var color = Index % 2 != 0 ? FigureColors.White : FigureColors.Black;
            var figurs = Cells.Where(i => i.Figure != null && i.Figure.Color == color).Select(i => i.Figure).ToList();
            return !figurs.SelectMany(i => i.PossibleMoves).Any();
        }

        public static bool operator ==(Board b1, Board b2)
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (!(b1[i, j].Figure?.ToString() == b2[i, j].Figure?.ToString()))
                        return false;
            return true;
        }

        public float Evaluation()
        {

            float sum = 0;

            foreach (var cell in Cells)
            {
                if (cell.Figure != null)
                {
                    sum += cell.Figure.Weight;
                }
            }
            
            if (IsCheckMate)
            {
                if (Index % 2 == 0)
                    sum += 9999f;
                else
                    sum -= 9999f;
            }
            

            return sum;
        }

        public static bool operator !=(Board b1, Board b2)
        {
            return !(b1 == b2);
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

        private void SetupСhessBoard2()
        {
            //this[0, 0].Figure = new Rook(FigureColors.Black);
            this[5, 2].Figure = new Queen(FigureColors.Black);
            //this[0, 2].Figure = new Bishop(FigureColors.Black);
            //this[0, 3].Figure = new Queen(FigureColors.Black);
            this[0, 4].Figure = new King(FigureColors.Black, 1);
            //this[0, 5].Figure = new Bishop(FigureColors.Black);
            //this[0, 6].Figure = new Knight(FigureColors.Black);
            this[0, 7].Figure = new Rook(FigureColors.Black);

           // this[7, 0].Figure = new Rook(FigureColors.White);
            //this[7, 1].Figure = new Knight(FigureColors.White);
           // this[7, 2].Figure = new Bishop(FigureColors.White);
            this[7, 3].Figure = new Queen(FigureColors.White);
            this[7, 4].Figure = new King(FigureColors.White, 1);
            //this[7, 5].Figure = new Bishop(FigureColors.White);
           // this[7, 6].Figure = new Knight(FigureColors.White);
            //this[7, 7].Figure = new Rook(FigureColors.White);
        }

        private void SetupСhessBoard3()
        {
            //this[0, 0].Figure = new Rook(FigureColors.Black);
            //this[5, 2].Figure = new Queen(FigureColors.Black);
            //this[0, 2].Figure = new Bishop(FigureColors.Black);
            //this[0, 3].Figure = new Queen(FigureColors.Black);
            this[0, 0].Figure = new King(FigureColors.Black, 1);
            //this[0, 5].Figure = new Bishop(FigureColors.Black);
            this[2, 0].Figure = new Knight(FigureColors.Black);
            //this[0, 7].Figure = new Rook(FigureColors.Black);

            // this[7, 0].Figure = new Rook(FigureColors.White);
            //this[7, 1].Figure = new Knight(FigureColors.White);
            // this[7, 2].Figure = new Bishop(FigureColors.White);
            //this[7, 3].Figure = new Queen(FigureColors.White);
            this[2, 1].Figure = new King(FigureColors.White, 1);
            //this[7, 5].Figure = new Bishop(FigureColors.White);
            // this[7, 6].Figure = new Knight(FigureColors.White);
            this[1, 1].Figure = new Rook(FigureColors.White);
        }

        private void SetupСhessBoard4()
        {
            this[2, 1].Figure = new Pawn(FigureColors.Black);
            //this[0, 0].Figure = new Rook(FigureColors.Black);
            //this[5, 2].Figure = new Queen(FigureColors.Black);
            //this[0, 2].Figure = new Bishop(FigureColors.Black);
            //this[0, 3].Figure = new Queen(FigureColors.Black);
            this[3, 3].Figure = new King(FigureColors.Black, 1);
            //this[0, 5].Figure = new Bishop(FigureColors.Black);
            this[1, 1].Figure = new Knight(FigureColors.Black);
            //this[0, 7].Figure = new Rook(FigureColors.Black);

            // this[7, 0].Figure = new Rook(FigureColors.White);
            this[0, 1].Figure = new Knight(FigureColors.White);
            // this[7, 2].Figure = new Bishop(FigureColors.White);
            this[5, 3].Figure = new Queen(FigureColors.White);
            this[1, 4].Figure = new King(FigureColors.White, 1);
            this[4, 3].Figure = new Pawn(FigureColors.White);
            //this[7, 5].Figure = new Bishop(FigureColors.White);
            // this[7, 6].Figure = new Knight(FigureColors.White);
            //this[1, 1].Figure = new Rook(FigureColors.White);
        }

        private void SetupСhessBoard7()
        {
            //this[2, 1].Figure = new Pawn(FigureColors.Black);
             this[0, 0].Figure = new Rook(FigureColors.Black);
            //this[5, 2].Figure = new Queen(FigureColors.Black);
            //this[0, 2].Figure = new Bishop(FigureColors.Black);
            //this[0, 3].Figure = new Queen(FigureColors.Black);
            this[0, 1].Figure = new King(FigureColors.Black, 1);
            this[1, 2].Figure = new Pawn(FigureColors.Black, 1);
            //this[0, 5].Figure = new Bishop(FigureColors.Black);
            //this[1, 1].Figure = new Knight(FigureColors.Black);
            //this[0, 7].Figure = new Rook(FigureColors.Black);

            // this[7, 0].Figure = new Rook(FigureColors.White);
            //this[0, 1].Figure = new Knight(FigureColors.White);
            this[5, 5].Figure = new Bishop(FigureColors.White);
            //this[5, 3].Figure = new Queen(FigureColors.White);
            this[1, 6].Figure = new Pawn(FigureColors.White, 1);
            this[0, 7].Figure = new King(FigureColors.White, 1);
            //this[4, 3].Figure = new Pawn(FigureColors.White);
            //this[7, 5].Figure = new Bishop(FigureColors.White);
            // this[7, 6].Figure = new Knight(FigureColors.White);
            //this[1, 1].Figure = new Rook(FigureColors.White);
        }

        private void SetupСhessBoard5()
        {
            // мат в 2 хода

            this[2, 0].Figure = new Pawn(FigureColors.Black);
            //this[0, 0].Figure = new Rook(FigureColors.Black);
            //this[5, 2].Figure = new Queen(FigureColors.Black);
            //this[0, 2].Figure = new Bishop(FigureColors.Black);
            //this[0, 3].Figure = new Queen(FigureColors.Black);
            this[2, 2].Figure = new King(FigureColors.Black, 1);
            //this[0, 5].Figure = new Bishop(FigureColors.Black);
            //this[1, 1].Figure = new Knight(FigureColors.Black);
            //this[0, 7].Figure = new Rook(FigureColors.Black);

            // this[7, 0].Figure = new Rook(FigureColors.White);
            //this[0, 1].Figure = new Knight(FigureColors.White);
            // this[7, 2].Figure = new Bishop(FigureColors.White);
            this[6, 6].Figure = new Queen(FigureColors.White);
            this[7, 1].Figure = new King(FigureColors.White, 1);
            this[3, 0].Figure = new Pawn(FigureColors.White);
            this[4, 3].Figure = new Pawn(FigureColors.White);
            this[6, 7].Figure = new Bishop(FigureColors.White);
            // this[7, 6].Figure = new Knight(FigureColors.White);
            this[3, 3].Figure = new Rook(FigureColors.White);
        }
    }
}
