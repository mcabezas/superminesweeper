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
            return _instance ??= new GameBll();
        }

        private GameBll()
        {
            gameDal = GameDal.Instance();
            arenaDal = ArenaDal.Instance();
        }

        public Game FindById(int id)
        {
            return gameDal.FindById(id);
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

            var cell = FindCell(action.PositionX, action.PositionY, game.Arena);

            if (cell == null)
            {
                return new ActionResponse { ErrorMessage = "Cell does not exist!"};
            }

            if (cell.Player != null)
            {
                return new ActionResponse { ErrorMessage = "Cell is already revealed!"};
            }
            
            //Push Cell
            PressCell(cell, game.Arena, game.NextMove);
            
            game.NextMove = GetNextPlayer(game.NextMove, game.Players);
            
            arenaDal.Update(game.Arena);
            gameDal.Update(game);
            
            return new ActionResponse{HasMine = cell.IsMine, NextMove = game.NextMove.Id};
        }

        private Cell FindCell(int positionX, int positionY, Arena arena)
        {
            foreach (var current in arena.Cells)
            {
                if (current.PositionX == positionX
                    && current.PositionY == positionY)
                {
                    return current;
                }
            }

            return null;
        }
        
        private void PressCell(Cell toBePushed, Arena arena, Player player)
        {
            var enumerator = arena.Cells.GetEnumerator();
            while(enumerator.MoveNext())
            {
                if (enumerator.Current.PositionX == toBePushed.PositionX
                    && enumerator.Current.PositionY == toBePushed.PositionY)
                {
                    enumerator.Current.Player = player;
                }
            }
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