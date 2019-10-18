using Minesweeper.Layer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper.Views
{
    internal class ArenaView : UserControl
    {
        private Control background;
        private Control board;

        public ArenaView() {
            this.Location = new Point(0, 0);
            this.Size = new Size(500, 500);
            this.Visible = true;

            this.board = new ArenaBoardLayer(9, 9);
            this.Controls.Add(board);

            this.background = new ArenaBackgroundLayer();
            this.Controls.Add(background);
        }
    }
}