using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using BE;
using DAL;

namespace BLL
{
    public class GameBll : IGame
    {
        private static GameBll _instance;
        private readonly GameDal gameDal;
        private readonly ArenaDal arenaDal;
        private readonly PlayerDal playerDal;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static GameBll Instance()
        {
        {
            if (_instance != null)
            {
                return _instance;
            }

            _instance = new GameBll();
            return _instance;
        }        }

        private GameBll()
        {
            gameDal = GameDal.Instance();
            arenaDal = ArenaDal.Instance();
            playerDal = PlayerDal.Instance();
        }

        public Game FindById(int id)
        {
            var game = gameDal.FindById(id);

            game.Arena = arenaDal.FindById(game.Arena.Id);
            return game;
        }

        public int CreateGame(List<Player> players)
        {
            var game = GameFactory.CreateRandomGame(players);
            game.Arena.Id = arenaDal.Create(game.Arena);
            return gameDal.Create(game);
        }
        
        public ActionResponse PressCell(ActionRequest action)
        {
            var game = gameDal.FindById(action.GameId);
            
            if (action.PositionX < 1 && action.PositionX > game.Arena.Height)
            {
                return new ActionResponse { ErrorMessage = "Position X is out of boundaries!"};
            }
            
            if (action.PositionY < 1 && action.PositionY > game.Arena.Weight)
            {
                return new ActionResponse { ErrorMessage = "Position Y is out of boundaries!"};
            }

            var cell = arenaDal.GetCell(game.Arena.Id, action.PositionX, action.PositionY);

            if (cell.Player != null)
            {
                return new ActionResponse { ErrorMessage = "Cell is already revealed!"};
            }

            arenaDal.UpdateCellPlayer(game.Arena.Id, action.PositionX, action.PositionY, game.NextMove.Id);

            if(!cell.IsMine)
            {
                game.NextMove = GetNextPlayer(game.NextMove, game.Players);
            }

            gameDal.Update(game);

            var nextPlayer = playerDal.FindById(game.NextMove.Id);
            
            return new ActionResponse{HasMine = cell.IsMine, NextMove = nextPlayer.Id, NextMoveName = nextPlayer.NickName, NearMines = cell.NearMines};
        }

        private Player GetNextPlayer(Player currentPlayer, List<Player> players)
        {
            foreach (var player in players)
            {
                if (player.Id != currentPlayer.Id) return player;
            }

            return null;
        }


        public void Delete(int id)
        {
            gameDal.Delete(id);
        }
        
    }
}