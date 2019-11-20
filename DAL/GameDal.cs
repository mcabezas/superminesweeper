using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using BE;

namespace DAL
{
    public class GameDal
    {
        private static GameDal _instance;

        private GameDal()
        {
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static GameDal Instance()
        {
            return _instance ??= new GameDal();
        }

        public Game FindById(int id)
        {
            IDatabase database = Database.Instance();

            var parameters = new List<SqlParameter> {new SqlParameter("@ID", id)};
            var gameDs = database.ExecuteQuery(@"SELECT * from GAME WHERE ID = @ID", parameters.ToArray());

            var gameParameters = new List<SqlParameter> {new SqlParameter("@ID", id)};
            var cellsDs = database.ExecuteQuery(@"SELECT * from GAME_PLAYER WHERE GAME_ID = @ID", gameParameters.ToArray());
            
            return GameMapper.ToEntity(gameDs, cellsDs);
        }

        public int Create(Game game)
        {
            IDatabase database = Database.Instance();

            var transaction = database.BeginTransaction();
            var gameId = -1;
            try
            {
                gameId = InsertGame(game, database, transaction);
                InsertGamePlayers(game.Players, gameId, database, transaction);
                transaction.Commit();
                Console.WriteLine("Game [" + gameId +"] successfully persisted.");
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an issue persisting Game: " + e.Message);
                transaction.Rollback();
            }

            return gameId;
        }

        public void Delete(int id)
        {
            IDatabase database = Database.Instance();
            database.ExecuteNonQuery("DELETE GAME WHERE ID=@ID", 
                new List<SqlParameter> {new SqlParameter("@ID", id)}.ToArray());
        }

        public int InsertGame(Game game, IDatabase database, SqlTransaction transaction)
        {
            BuildGameParams(game, out var columns, out var values, out var parameters);
            return (int) database.ExecuteScalar("INSERT INTO GAME(" + columns + ") " +
                "OUTPUT INSERTED.ID VALUES (" + values + ")", parameters.ToArray(), transaction);
        }
        
        public int InsertGame(Game game, IDatabase database)
        {
            BuildGameParams(game, out var columns, out var values, out var parameters);
            return (int) database.ExecuteScalar("INSERT INTO GAME(" + columns + ") " +
                "OUTPUT INSERTED.ID VALUES (" + values + ")", parameters.ToArray());
        }

        private void BuildGameParams(Game game, out string columns, out string values, out List<SqlParameter> parameters)
        {
            columns = "";
            values = "";

            parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@STARTING_DATE", game.StartingDate));
            columns += "STARTING_DATE";
            values += "@STARTING_DATE";

            if (game.Arena != null)
            {
                parameters.Add(new SqlParameter("@ARENA_ID", game.Arena.Id));
                columns += ",ARENA_ID";
                values += ",@ARENA_ID";
            }

            var player1 = game.Players.GetEnumerator().Current;
            if (player1 != null)
            {
                parameters.Add(new SqlParameter("@NEXT_MOVE", player1.Id));
                columns += ",NEXT_MOVE";
                values += ",@NEXT_MOVE";
            }

        }
        
        public void InsertGamePlayers(IEnumerable<Player> players, int gameId, IDatabase database, SqlTransaction transaction)
        {
            foreach (var player in players)
            {
                BuildGamePlayersParameters(gameId, player, out var columns, out var values, out var parameters);
                database.ExecuteNonQuery("INSERT INTO GAME_PLAYER(" + columns + ") " +
                    " VALUES (" + values + ")", parameters.ToArray(), transaction);
            }
        }
        
        public void InsertGamePlayers(IEnumerable<Player> players, int gameId, IDatabase database)
        {
            foreach (var player in players)
            {
                BuildGamePlayersParameters(gameId, player, out var columns, out var values, out var parameters);
                database.ExecuteNonQuery("INSERT INTO GAME_PLAYER(" + columns + ") " +
                                         " VALUES (" + values + ")", parameters.ToArray());
            }
        }
        
        private void BuildGamePlayersParameters(int gameId, Player player, 
            out string columns, out string values, out List<SqlParameter> parameters)
        {
            columns = "";
            values = "";

            parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@GAME_ID", gameId));
            columns += "GAME_ID";
            values += "@GAME_ID";

            parameters.Add(new SqlParameter("@PLAYER_ID", player.Id));
            columns += ",PLAYER_ID";
            values += ",@PLAYER_ID";
        }

    }
}