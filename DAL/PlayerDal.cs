using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using BE;

namespace DAL
{
    public class PlayerDal : IPlayerDal
    {
        private static PlayerDal _instance;

        private PlayerDal()
        {
        }
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static PlayerDal Instance()
        {
            {
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = new PlayerDal();
                return _instance;
            }
        }

        public Player FindById(int id)
        {
            IDatabase database = Database.Instance();

            var parameters = new List<SqlParameter> {new SqlParameter("@ID", id)};
            var dataSet = database.ExecuteQuery(@"SELECT * from PLAYER WHERE ID = @ID", parameters.ToArray());
            return PlayerMapper.ToEntity(dataSet);
        }
        
        public int Create(Player player)
        {
            var columns = "";
            var values = "";
            
            IDatabase database = Database.Instance();
            var parameters = new List<SqlParameter> {new SqlParameter("@NICKNAME", player.NickName)};

            columns += "NICKNAME";
            values += "@NICKNAME";
            
            return (int) database.ExecuteScalar(
                "INSERT INTO PLAYER(" + columns + ") " +
                "OUTPUT INSERTED.ID VALUES (" + values + ")", 
                parameters.ToArray());
        }

        public void Update(Player player)
        {
            IDatabase database = Database.Instance();
            var parameters = new List<SqlParameter> {new SqlParameter("@NICKNAME", player.NickName)};

            database.ExecuteNonQuery(@"UPDATE PLAYER SET NICKNAME = @NICKNAME", 
                parameters.ToArray());
        }

        public void Delete(int id)
        {
            IDatabase database = Database.Instance();

            var parameters = new List<SqlParameter> {new SqlParameter("@ID", id)};
            database.ExecuteNonQuery(@"DELETE PLAYER WHERE ID = @ID", parameters.ToArray());
        }
    }
}