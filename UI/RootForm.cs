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

namespace UI
{
    internal partial class RootForm : Form
    {
        private ArenaView arenaView;

        public RootForm()
        {
            InitializeComponent();
            ArenaView arenaView = new ArenaView();
            this.Controls.Add(arenaView);
        }
    }
}
