using System;
using System.Data;
using BE;

namespace DAL
{
    public static class GameMapper
    {
        public static Game ToEntity(DataSet gameDs, DataSet playersDs)
        {
            Game game = new Game();
            foreach (DataRow row in gameDs.Tables[0].Rows)
            {
                game.Id = int.Parse(row["id"].ToString());
                
                if (row["arena_id"].ToString() != "")
                {
                    game.Arena = new Arena{Id = int.Parse(row["arena_id"].ToString())};
                }

                if (row["winner"].ToString() != "")
                {
                    game.Winner = new Player {Id = int.Parse(row["winner"].ToString())};
                }

                if (row["next_move"].ToString() != "")
                {
                    game.NextMove = new Player {Id = int.Parse(row["next_move"].ToString())};
                }

                if (row["starting_date"].ToString() != "")
                {
                    game.StartingDate = DateTime.Parse(row["starting_date"].ToString());
                }

                if (row["ending_date"].ToString() != "")
                {
                    game.EndingDate = DateTime.Parse(row["ending_date"].ToString());
                }
            }

            foreach (DataRow row in playersDs.Tables[0].Rows)
            {
                var player = new Player()
                {
                    Id = int.Parse(row["player_id"].ToString()),
                };
                game.Players.Add(player);
            }

            return game;
        }
    }
}