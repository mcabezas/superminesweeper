using Minesweeper.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BE;
using BLL;

namespace UI
{
    internal partial class RootForm : Form
    {
        private ArenaView arenaView;

        public RootForm(RootController controller)
        {
            InitializeComponent();
            var players = controller.CreateDefaultPlayers();
            var gameID = GameBll.Instance().CreateGame(players);
            var game = GameBll.Instance().FindById(gameID);
            ArenaView arenaView = new ArenaView(controller, game, players);
            this.Controls.Add(arenaView);
        }

    }
}
