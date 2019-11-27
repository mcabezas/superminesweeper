using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BE;
using BLL;

namespace UI
{
    public class RootController
    {
        public List<Player> CreateDefaultPlayers()
        {
            var pBll = PlayerBll.Instance();
            var player1 = new Player { NickName = "player1" };
            var player2 = new Player { NickName = "player2" };
            player1.Id = pBll.CreatePlayer(player1);
            player2.Id = pBll.CreatePlayer(player2);
            return new List<Player> { player1, player2 };
        }

        public ActionResponse PressCell(int gameID, int x, int y) {
            return GameBll.Instance().PressCell(new ActionRequest { GameId = gameID, PositionX = x, PositionY = y });
        } 

    }
}
