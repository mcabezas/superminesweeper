using System.Data;
using BE;

namespace DAL
{
    public static class ArenaMapper
    {
        public static Arena ToArenaEntity(DataSet arenaDs, DataSet cellsDs)
        {
            var arena = new Arena();
            foreach (DataRow row in arenaDs.Tables[0].Rows)
            {
                arena.Id = int.Parse(row["id"].ToString());
                arena.Height = int.Parse(row["height"].ToString());
                arena.Weight = int.Parse(row["weight"].ToString());
            }
            
            foreach (DataRow row in cellsDs.Tables[0].Rows)
            {
                var cell = new Cell
                {
                    PositionX = int.Parse(row["position_x"].ToString()),
                    PositionY = int.Parse(row["position_y"].ToString()),
                    IsMine = row["is_mine"].ToString() == "1"
                };
                arena.Cells.Add(cell);
            }

            return arena;
        }
        
        public static Cell ToCelEntity(DataSet cellDs, DataSet nearMinesDs)
        {
            var cell = new Cell();
            foreach (DataRow row in cellDs.Tables[0].Rows)
            {
                if (row["player_id"].ToString() != "")
                {
                    cell.Player = new Player {Id = int.Parse(row["player_id"].ToString())};
                }
                cell.IsMine = row["is_mine"].ToString() == "1";
            }

            foreach (DataRow row in nearMinesDs.Tables[0].Rows)
            {
                cell.NearMines = int.Parse(row["near_mines"].ToString());
            }

            return cell;
        }
    }
}