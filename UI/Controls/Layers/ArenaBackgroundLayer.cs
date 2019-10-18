using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Minesweeper.Layer
{
    class ArenaBackgroundLayer : UserControl
    {
        public ArenaBackgroundLayer() {
            this.Location = new Point(0, 0);
            this.Size = new Size(500, 500);
            this.Visible = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graphics = e.Graphics;
            graphics.Clear(Color.FromArgb(2, 6, 255));
        }
    }
}
