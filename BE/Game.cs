using System;
using System.Collections.Generic;

namespace BE
{
    public class Game
    {
        public Game()
        {
            Players = new List<Player>();
        }
        
        public int Id { get; set; }

        public List<Player> Players { get; set; }
        
        public Player Winner { get; set; }

        public Player NextMove { get; set; }

        public DateTime StartingDate { get; set; }
        
        public DateTime? EndingDate { get; set; }
        
        public Arena Arena { get; set; }
    }
}
