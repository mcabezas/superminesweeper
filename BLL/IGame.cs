using System.Collections.Generic;
using BE;

namespace BLL
{
    public interface IGame
    {
        int CreateGame(List<Player> players);
        Game FindById(int id);
        ActionResponse PressCell(ActionRequest actionRequest);
    }
}