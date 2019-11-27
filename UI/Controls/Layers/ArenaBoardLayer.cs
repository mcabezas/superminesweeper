using Minesweeper.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UI;

namespace Minesweeper.Layer
{
    internal class ArenaBoardLayer : UserControl
    {
        private int cellWeight = 20;
        private int cellHeight = 20;
        private int rows;
        private int columns;
        private Cell[,] cells;
        private RootController controller;

        public ArenaBoardLayer(RootController controller, BE.Game game)
        {
            this.controller = controller;
            this.rows = game.Arena.Height;
            this.columns = game.Arena.Weight;
            this.cells = new Cell[rows, columns];
            this.Location = new Point(200, 200);
            this.Size = new Size(columns * cellWeight, rows * cellHeight);
            this.Visible = true;

            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < columns; columnIndex++)
                {
                    int positionX = (columnIndex * cellWeight);
                    int positionY = (rowIndex * cellHeight);

                    cells[rowIndex, columnIndex] = new Cell(
                        controller,
                        rowIndex, columnIndex, game.Id,
                        new Point( positionX , positionY),
                        new Size(cellWeight, cellHeight));

                    this.Controls.Add(cells[rowIndex, columnIndex]);
                }
            }
        }
    }
}