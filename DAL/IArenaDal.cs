
using BE;

namespace DAL
{
    public interface IArenaDal : IDal<int, Arena>
    {
        Cell GetCell(int arenaId, int positionX, int positionY);
        void UpdateCellPlayer(int arenaId, int positionX, int positionY, int playerId);
    }
}