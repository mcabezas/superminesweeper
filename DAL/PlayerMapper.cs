using System;
using System.Data;
using System.Net.NetworkInformation;
using BE;

namespace DAL
{
    public static class PlayerMapper
    {
        public static Player ToEntity(DataSet dataSet)
        {
            var player = new Player();
            var table = dataSet.Tables[0];
            foreach (DataRow row in table.Rows)
            {
                player.Id = Int32.Parse(row["id"].ToString());
                player.NickName = row["nickname"].ToString();
            }
            return player;
        }
    }
}