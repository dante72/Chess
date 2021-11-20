﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.Figure;

namespace Chess
{
    public class ViewModel : NotifyPropertyChanged
    {
        public Board СhessBoard { set; get; } = new Board();
        public ObservableCollection<Cell> Cells { set; get; }

        private Cell selectedItem;
        
        /// <summary>
        /// Выбранная клетка на доске (вся локика доски)
        /// </summary>
        public Cell SelectedItem
        {
            set
            {
                if (selectedItem != null)
                    selectedItem.IsSelected = false;
                
                if (value.IsMarked == false)
                    selectedItem?.Figure?.GetPossibleMoves()
                        .ForEach(a => a.IsMarked = false);

                if (value?.IsMarked == true)
                { 
                    selectedItem?.Figure?.GetPossibleMoves()
                        .ForEach(a => a.IsMarked = false);
                    value.Figure = selectedItem.Figure;
                    selectedItem.Figure = null;

                }
                else
                {
                    selectedItem = value;
                    selectedItem?.Figure?.GetPossibleMoves()
                    .ForEach(a => a.IsMarked = true);

                    selectedItem.IsSelected = true;
                }
            }
            get => selectedItem;
        
        }

        public ViewModel()
        {
            SetupСhessBoard();
        }

        private void SetupСhessBoard()
        {
            СhessBoard[0, 0] = new Rook(FigureColors.Black);
            СhessBoard[0, 1] = new Knight(FigureColors.Black);
            СhessBoard[0, 2] = new Bishop(FigureColors.Black);
            СhessBoard[0, 3] = new Queen(FigureColors.Black);
            СhessBoard[0, 4] = new King(FigureColors.Black);
            СhessBoard[0, 5] = new Bishop(FigureColors.Black);
            СhessBoard[0, 6] = new Knight(FigureColors.Black);
            СhessBoard[0, 7] = new Rook(FigureColors.Black);
            for (int i = 0; i < 8; i++)
            {
                СhessBoard[1, i] = new Pawn(FigureColors.Black);
                СhessBoard[6, i] = new Pawn(FigureColors.White);
            }
            СhessBoard[7, 0] = new Rook(FigureColors.White);
            СhessBoard[7, 1] = new Knight(FigureColors.White);
            СhessBoard[7, 2] = new Bishop(FigureColors.White);
            СhessBoard[7, 3] = new Queen(FigureColors.White);
            СhessBoard[7, 4] = new King(FigureColors.White);
            СhessBoard[7, 5] = new Bishop(FigureColors.White);
            СhessBoard[7, 6] = new Knight(FigureColors.White);
            СhessBoard[7, 7] = new Rook(FigureColors.White);
        }
    }
}
