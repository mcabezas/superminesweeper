namespace BLL
{
    public class ActionResponse
    {
        public bool HasMine { get; set; }
        public int NextMove { get; set; }
        public int Winner { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}