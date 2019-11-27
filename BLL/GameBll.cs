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
            game.Arena = arenaDal.FindById(game.Arena.Id);
            
            if (action.PositionX < 1 && action.PositionX > game.Arena.Height)
            {
                return new ActionResponse { ErrorMessage = "Position X is out of boundaries!"};
            }
            
            if (action.PositionY < 1 && action.PositionY > game.Arena.Weight)
            {
                return new ActionResponse { ErrorMessage = "Position Y is out of boundaries!"};
            }

            var cellPos = FindCell(action.PositionX, action.PositionY, game.Arena);

            if (game.Arena.Cells[cellPos].Player != null)
            {
                return new ActionResponse { ErrorMessage = "Cell is already revealed!"};
            }

            game.Arena.Cells[cellPos].Player = game.NextMove;


            game.NextMove = GetNextPlayer(game.NextMove, game.Players);
            
            arenaDal.Update(game.Arena);
            gameDal.Update(game);
            
            return new ActionResponse{HasMine = game.Arena.Cells[cellPos].IsMine, NextMove = game.NextMove.Id};
        }

        private int FindCell(int positionX, int positionY, Arena arena)
        {
            for(int i=0; i < arena.Cells.Count; i++)
            {
                if (arena.Cells[i].PositionX == positionX
                    && arena.Cells[i].PositionY == positionY)
                {
                    return i;
                }
            }
            return 0;
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