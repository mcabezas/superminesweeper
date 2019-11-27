using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using BE;
using DAL;

namespace BLL
{
    public class PlayerBll : IPlayer
    {
        private static PlayerBll _instance;
        private readonly PlayerDal playerDal;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static PlayerBll Instance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            _instance = new PlayerBll();
            return _instance;
        }

        private PlayerBll()
        {
            playerDal = PlayerDal.Instance();
        }

        public Player FindById(int id)
        {
            return playerDal.FindById(id);
        }

        public int CreatePlayer(Player player)
        {
            return playerDal.Create(player);
        }

        public void Delete(int id)
        {
            playerDal.Delete(id);
        }
        
    }
}