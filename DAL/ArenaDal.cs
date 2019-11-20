using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using BE;

namespace DAL
{
    public class ArenaDal : IArenaDal
    {
        private static ArenaDal _instance;

        private ArenaDal()
        {
        }
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ArenaDal Instance()
        {
            return _instance ??= new ArenaDal();
        }

        public Arena FindById(int id)
        {
            IDatabase database = Database.Instance();

            var parameters = new List<SqlParameter> {new SqlParameter("@ID", id)};
            var arenaDs = database.ExecuteQuery(@"SELECT * from ARENA WHERE ID = @ID", parameters.ToArray());

            var cellParameters = new List<SqlParameter> {new SqlParameter("@ID", id)};
            var cellsDs = database.ExecuteQuery(@"SELECT * from ARENA_CELL WHERE ARENA_ID = @ID", cellParameters.ToArray());
            
            return ArenaMapper.ToEntity(arenaDs, cellsDs);
        }
        
        public int Create(Arena arena)
        {
            IDatabase database = Database.Instance();
            var transaction = database.BeginTransaction();

            try
            {
                var columns = "";
                var values = "";

                var arenaId = InsertArena(arena, columns, values, database, transaction);
                InsertCell(arena.Cells, arenaId, database, transaction);
                Console.WriteLine("Arena [" + arenaId +"] successfully persisted.");
                transaction.Commit();
                return arenaId;
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an issue persisting Arena: " + e.Message);
                transaction.Rollback();
                return -1;
            }
        }
        
        public static int InsertArena(Arena arena, string columns, string values, IDatabase database, SqlTransaction transaction)
        {
            var parameters = BuildArenaParameters(arena, ref columns, ref values);
            return (int) database.ExecuteScalar("INSERT INTO ARENA(" + columns + ") " +
                                                "OUTPUT INSERTED.ID VALUES (" + values + ")", parameters.ToArray(), transaction);
        }

        private static List<SqlParameter> BuildArenaParameters(Arena arena, ref string columns, ref string values)
        {
            var parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@WEIGHT", arena.Weight));
            columns += "WEIGHT";
            values += "@WEIGHT";

            parameters.Add(new SqlParameter("@HEIGHT", arena.Height));
            columns += ",HEIGHT";
            values += ",@HEIGHT";
            return parameters;
        }

        public void InsertCell(IEnumerable<Cell> cells, int arenaId, IDatabase database, SqlTransaction transaction)
        {
            foreach (var cell in cells)
            {
                BuildDiscoveredParameters(arenaId, cell, out var columns, out var values, out var parameters);
                database.ExecuteNonQuery("INSERT INTO ARENA_CELL(" + columns + ") " +
                    " VALUES (" + values + ")", parameters.ToArray(), transaction);
            }
        }
        
        private void BuildDiscoveredParameters(int arenaId, Cell cell, 
            out string columns, out string values, out List<SqlParameter> parameters)
        {
            columns = "";
            values = "";

            parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@ARENA_ID", arenaId));
            columns += "ARENA_ID";
            values += "@ARENA_ID";

            parameters.Add(new SqlParameter("@POSITION_X", cell.PositionX));
            columns += ",POSITION_X";
            values += ",@POSITION_X";

            parameters.Add(new SqlParameter("@POSITION_Y", cell.PositionY));
            columns += ",POSITION_Y";
            values += ",@POSITION_Y";

            if (cell.Player != null)
            {
                parameters.Add(new SqlParameter("@PLAYER_ID", cell.Player.Id));
                columns += ",PLAYER_ID";
                values += ",@PLAYER_ID";
            }

            if (cell.IsMine)
            {
                parameters.Add(new SqlParameter("@IS_MINE", 1));
                columns += ",IS_MINE";
                values += ",@IS_MINE";
            }
        }

        public void Update(Arena arena) {
            IDatabase database = Database.Instance();
            var transaction = database.BeginTransaction();
            
            try
            {
                UpdateArenaTable(arena, database, transaction);
                UpdateArenaCellTable(arena, database, transaction);
                transaction.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an issue updating Arena: " + e.Message);
                transaction.Rollback();
            }
        }

        public void Delete(int id)
        {
            IDatabase database = Database.Instance();
            var transaction = database.BeginTransaction();
            
            try
            {
                DeleteArenaTable(id, database, transaction);
                DeleteArenaCellTable(id, database, transaction);
                transaction.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an issue deleting Arena: " + e.Message);
                transaction.Rollback();
            }
        }

        private static void UpdateArenaTable(Arena arena, IDatabase database, SqlTransaction transaction)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@WEIGHT", arena.Weight), new SqlParameter("@HEIGHT", arena.Height)
            };

            database.ExecuteNonQuery("UPDATE ARENA SET WEIGHT=@WEIGHT, HEIGHT=@HEIGHT"
                , parameters.ToArray(), transaction);
        }

        private void UpdateArenaCellTable(Arena arena, IDatabase database, SqlTransaction transaction)
        {
            DeleteArenaCellTable(arena.Id, database, transaction);
            InsertCell(arena.Cells, arena.Id, database, transaction);
        }

        private void DeleteArenaCellTable(int id, IDatabase database, SqlTransaction transaction)
        {
            var parameters = new List<SqlParameter> {
                new SqlParameter("@ID", id)
            };
            database.ExecuteNonQuery("DELETE ARENA_CELL WHERE ARENA_ID = @ID"
                , parameters.ToArray(), transaction);
        }
        
        private void DeleteArenaTable(int id, IDatabase database, SqlTransaction transaction)
        {
            var parameters = new List<SqlParameter> {
                new SqlParameter("@ID", id)
            };
            database.ExecuteNonQuery("DELETE ARENA WHERE ID = @ID"
                , parameters.ToArray(), transaction);
        }
    }
}