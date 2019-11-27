using Minesweeper.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UI;

namespace Minesweeper.Controls
{
    class Cell : UserControl
    {
        private bool flagged = false;
        private readonly int row;
        private readonly int column;
        private readonly int gameId;
        private RootController controller;

        private Graphics graphics;

        public Cell(RootController controller, int row, int column, int gameId, Point location, Size size)
        {
            this.controller = controller;
            this.row = row;
            this.column = column;
            this.gameId = gameId;
            this.Location = location;
            this.Size = size;
            this.Visible = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            graphics = e.Graphics;
            graphics.DrawImage(ImageResource.GetUnreveledBlankSquare(this.Size), 0, 0);

            /*
                        Graphics graphics = e.Graphics;
                        Pen RedPen = new Pen(Color.Red, 3);
                        Brush SolidAzureBrush = Brushes.Aqua;

                        Rectangle RectangleArea = new Rectangle(new Point(0,0), this.Size);

                        graphics.FillRectangle(SolidAzureBrush, RectangleArea);
                        graphics.DrawRectangle(RedPen, RectangleArea);
            */

        }

        protected override void OnMouseClick(MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                MessageBox.Show($"Right click on Row[{row}] Column[{column}]" );
            }
            Graphics graphics = this.CreateGraphics();
            var result = controller.PressCell(gameId, column, row);

            if (result.ErrorMessage != null) return;

            if (result.HasMine) {
                graphics.DrawImage(ImageResource.GetReveledMineSquare(this.Size), 0, 0);
                return;
            }
            graphics.DrawImage(ImageResource.GetReveledBlankSquare(this.Size), 0, 0);
            using (Font arialFont = new Font("Arial", 10))
            {
                PointF location = new PointF(5f, 2.5f);
                graphics.DrawString(result.NearMines.ToString(), arialFont, Brushes.Blue, location);
            }
        }

    }
}