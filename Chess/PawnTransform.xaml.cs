using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Chess.Figure;

namespace Chess
{
    /// <summary>
    /// Логика взаимодействия для PawnTransform.xaml
    /// </summary>
    public partial class PawnTransform : Window
    {
        private FigureColors color;
        public PawnTransform(FigureColors color)
        {
            InitializeComponent();
            this.color = color;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch ((string)(sender as Button).Content)
            {
                case "Queen":
                    DataContext = new Queen(color);
                    break;
                case "Knight":
                    DataContext = new Knight(color);
                    break;
                case "Bishop":
                    DataContext = new Bishop(color);
                    break;
                case "Rook":
                    DataContext = new Rook(color);
                    break;
            }
            Close();
        }
    }
}
