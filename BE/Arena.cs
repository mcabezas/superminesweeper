using System.Collections.Generic;

namespace BE
{
    public class Arena
    {
        public Arena()
        {
            Cells = new List<Cell>();
        }

        public int Id { get; set; }

        public int Weight { get; set; }

        public int Height { get; set; }
        
        public List<Cell> Cells { get; set; }

    }
}
