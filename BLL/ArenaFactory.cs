using System.Collections.Generic;
using System;
using BE;

namespace BLL
{
    public class ArenaFactory
    {
        public static Arena CreateRandomArena()
        {
            var boundaryX = Predefined.RandomInt(10, 15);
            var boundaryY = Predefined.RandomInt(10, 15);

            var cells = new List<Cell>();
            for (var x = 0; x < boundaryX; x++)
            {
                for (var y = 0; y < boundaryY; y++)
                {
                    bool random = Predefined.RandomInt(0, 5) == 0;
                    cells.Add(new Cell(x, y, random));
                }
            }
            
            return new Arena {Height = boundaryX, Weight = boundaryY, Cells = cells};
        }
    }
}