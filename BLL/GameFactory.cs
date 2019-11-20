using System;
using System.Collections.Generic;
using System.Linq;
using BE;

namespace BLL
{
    public class GameFactory
    {
        public static Game CreateRandomGame(List<Player> players)
        { 
            return new Game {
                Players = players,
                NextMove = GetRandomFirstMovePlayer(players),
                StartingDate = DateTime.Today,
                Arena = ArenaFactory.CreateRandomArena()
            };
        }

        private static Player GetRandomFirstMovePlayer(IReadOnlyCollection<Player> players)
        {
            var playerPosition = Predefined.RandomInt(0, players.Count() - 1);
            return players.ElementAt(playerPosition);
        }
    }
}