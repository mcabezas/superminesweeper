using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using BE;

namespace DAL
{
    public class GameDal : IGameDal
    {
        private static GameDal _instance;

        private GameDal()
        {
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static GameDal Instance()
        {
            {
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = new GameDal();
                return _instance;
            }
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
                gameId = InsertGameTable(game, database, transaction);
                InsertGamePlayerTable(game.Players, gameId, database, transaction);
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

        public void Update(Game game)
        {
            IDatabase database = Database.Instance();

            var transaction = database.BeginTransaction();
            try
            {
                UpdateGameTable(game, database, transaction);
                UpdateGamePlayerTable(game.Players, game.Id, database, transaction);
                transaction.Commit();
                Console.WriteLine("Game [" + game.Id +"] successfully updated.");
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an issue updating Game: " + e.Message);
                transaction.Rollback();
            }
        }
        
        public void UpdateGameTable(Game game, IDatabase database, SqlTransaction transaction)
        {
            var parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@ID", game.Id));
            parameters.Add(new SqlParameter("@NEXT_MOVE", game.NextMove.Id));
            parameters.Add(new SqlParameter("@STARTING_DATE", game.StartingDate));

            parameters.Add(game.EndingDate != null
                ? new SqlParameter("@ENDING_DATE", game.EndingDate)
                : new SqlParameter("@ENDING_DATE", DBNull.Value));
            
            parameters.Add(game.Winner != null
                ? new SqlParameter("@WINNER", game.Winner.Id)
                : new SqlParameter("@WINNER", DBNull.Value));

            parameters.Add(game.Arena != null
                ? new SqlParameter("@ARENA_ID", game.Arena.Id)
                : new SqlParameter("@ARENA_ID", DBNull.Value));

            database.ExecuteNonQuery("UPDATE GAME SET WINNER=@WINNER" + 
                                     " , NEXT_MOVE=@NEXT_MOVE, STARTING_DATE=@STARTING_DATE, ENDING_DATE=@ENDING_DATE, ARENA_ID=@ARENA_ID" + 
                                     " WHERE ID=@ID", 
                parameters.ToArray(), transaction);
        }

        public void UpdateGamePlayerTable(IEnumerable<Player> players, int gameId, IDatabase database, SqlTransaction transaction)
        {
            DeleteGamePlayerTable(gameId, database, transaction);
            InsertGamePlayerTable(players, gameId, database, transaction);
        }

        private static void DeleteGamePlayerTable(int gameId, IDatabase database, SqlTransaction transaction)
        {
            database.ExecuteNonQuery("DELETE GAME_PLAYER WHERE GAME_ID=@ID",
                new List<SqlParameter> {new SqlParameter("@ID", gameId)}.ToArray(), transaction);
        }

        public void Delete(int id)
        {
            IDatabase database = Database.Instance();
            
            database.ExecuteNonQuery("DELETE GAME_PLAYER WHERE GAME_ID=@ID", 
                new List<SqlParameter> {new SqlParameter("@ID", id)}.ToArray());

            database.ExecuteNonQuery("DELETE GAME WHERE ID=@ID", 
                new List<SqlParameter> {new SqlParameter("@ID", id)}.ToArray());
        }

        public int InsertGameTable(Game game, IDatabase database, SqlTransaction transaction)
        {
            BuildGameParams(game, out var columns, out var values, out var parameters);
            return (int) database.ExecuteScalar("INSERT INTO GAME(" + columns + ") " +
                "OUTPUT INSERTED.ID VALUES (" + values + ")", parameters.ToArray(), transaction);
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

            if (game.NextMove != null)
            {
                parameters.Add(new SqlParameter("@NEXT_MOVE", game.NextMove.Id));
                columns += ",NEXT_MOVE";
                values += ",@NEXT_MOVE";
            }

        }
        
        public void InsertGamePlayerTable(IEnumerable<Player> players, int gameId, IDatabase database, SqlTransaction transaction)
        {
            foreach (var player in players)
            {
                BuildGamePlayersParameters(gameId, player, out var columns, out var values, out var parameters);
                database.ExecuteNonQuery("INSERT INTO GAME_PLAYER(" + columns + ") " +
                    " VALUES (" + values + ")", parameters.ToArray(), transaction);
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