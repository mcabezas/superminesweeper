using Minesweeper.Layer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BE;
using UI;

namespace Minesweeper.Views
{
    internal class ArenaView : UserControl
    {
        private Control background;
        private Control board;
        private ArenaDashboardLayer dashboard;

        public ArenaView(RootController controller, Game game, List<Player> players) {
            this.Location = new Point(0, 0);
            this.Size = new Size(1000, 1000);
            this.Visible = true;

            this.dashboard = new ArenaDashboardLayer();
            this.Controls.Add(dashboard);

            this.board = new ArenaBoardLayer(dashboard, controller, game);
            this.Controls.Add(board);

            this.background = new ArenaBackgroundLayer();
            this.Controls.Add(background);
        }
    }
}