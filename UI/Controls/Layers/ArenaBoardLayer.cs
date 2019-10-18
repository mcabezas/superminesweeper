using Minesweeper.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Minesweeper.Layer
{
    internal class ArenaBoardLayer : UserControl
    {
        private int cellWeight = 20;
        private int cellHight = 20;
        private int rows;
        private int columns;
        private Cell[,] cells;

        public ArenaBoardLayer(int rows, int columns)
        {
            this.cells = new Cell[rows, columns];
            this.rows = rows;
            this.columns = columns;
            this.Location = new Point(200, 200);
            this.Size = new Size(columns * cellWeight, rows * cellHight);
            this.Visible = true;

            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < columns; columnIndex++)
                {
                    int positionX = (columnIndex * cellWeight);
                    int positionY = (rowIndex * cellHight);

                    cells[rowIndex, columnIndex] = new Cell(
                        rowIndex, columnIndex, false,
                        new Point( positionX , positionY),
                        new Size(cellWeight, cellHight));

                    this.Controls.Add(cells[rowIndex, columnIndex]);
                }
            }
        }

    }
}