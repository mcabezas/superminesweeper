using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Arena
    {
        private long id;

        public long Id
        {
            get { return id; }
            set { id = value; }
        }

        private int wheight;

        public int Wheight
        {
            get { return wheight; }
            set { wheight = value; }
        }

        private int hight;

        public int Hight
        {
            get { return hight; }
            set { hight = value; }
        }

        private Dictionary<Position, Player> mines;

        public Dictionary<Position, Player> Mines
        {
            get { return mines; }
            set { mines = value; }
        }

    }
}
