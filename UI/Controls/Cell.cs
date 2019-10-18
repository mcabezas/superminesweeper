using Minesweeper.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Minesweeper.Controls
{
    class Cell : UserControl
    {
        private bool flagged = false;
        private readonly int row;
        private readonly int column;
        private readonly bool hasMine;
        private Graphics graphics;

        public Cell(int row, int column, bool hasMine, Point location, Size size)
        {
            this.row = row;
            this.column = column;
            this.hasMine = hasMine;
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
            graphics.DrawImage(ImageResource.GetReveledBlankSquare(this.Size), 0, 0);
        }

    }
}