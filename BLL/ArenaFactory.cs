using System.Collections.Generic;
using System.Linq;
using BE;

namespace BLL
{
    public class ArenaFactory
    {
        public static Arena CreateRandomArena()
        {
            var boundaryX = Predefined.RandomInt(15, 30);
            var boundaryY = Predefined.RandomInt(15, 30);

            var cells = new List<Cell>();
            for (var x = 0; x < boundaryX; x++)
            {
                for (var y = 0; y < boundaryY; y++)
                {
                    cells.Add(new Cell(x, y,Predefined.RandomInt(0, 5)==1));
                }
            }
            
            return new Arena {Height = boundaryX, Weight = boundaryY, Cells = cells};
        }
    }
}