namespace BE
{
    public class Cell
    {
        public Cell() {}

        public Cell(int positionX, int positionY, bool isMine)
        {
            PositionX = positionX;
            PositionY = positionY;
            IsMine = isMine;
        }

        public int PositionX { get; set; }

        public int PositionY { get; set; }

        public bool IsMine { get; set; }
        
        public bool NearMines { get; set; }

        public Player Player { get; set; }

        private bool Equals(Cell other)
        {
            return PositionX == other.PositionX && PositionY == other.PositionY;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Cell) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (PositionX * 397) ^ PositionY;
            }
        }
    }

}
