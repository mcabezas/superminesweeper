namespace BLL
{
    public class ActionResponse
    {
        public bool HasMine { get; set; }
        public int NearMines { get; set; }
        public int NextMove { get; set; }
        public string NextMoveName { get; set; }
        public int Winner { get; set; }
        public string WinnerName { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}