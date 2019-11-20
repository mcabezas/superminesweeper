using System.Collections.Generic;
using BE;

namespace BLL
{
    public interface IGame
    {
        int CreateGame(List<Player> players);
        ActionResponse PressCell(ActionRequest actionRequest);
    }
}