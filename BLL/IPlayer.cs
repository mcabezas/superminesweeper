using System.Collections.Generic;
using BE;

namespace BLL
{
    public interface IPlayer
    {
        int CreatePlayer(Player player);
        Player FindById(int id);
        void Delete(int id);
    }
}