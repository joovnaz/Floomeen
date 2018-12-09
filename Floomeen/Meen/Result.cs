namespace Floomeen.Meen
{
    public class Result
    {
        public Result()
        {
            
        }

        public Result(bool success)
        {
            Success = success;
        }

        public bool Success { get; set; }

        public string  Message { get; set; }
    }
}
