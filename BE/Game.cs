using System;
using System.Collections;
using System.Collections.Generic;

namespace Model
{
    public class Game
    {
        private long id;

        public long Id
        {
            get { return id; }
            set { id = value; }
        }

        private IEnumerable<Player> players;

        public IEnumerable<Player> Players
        {
            get { return players; }
            set { players = value; }
        }

        private Arena arena;

        public Arena Arena
       {
            get { return arena; }
            set { arena = value; }
        }


    }
}
