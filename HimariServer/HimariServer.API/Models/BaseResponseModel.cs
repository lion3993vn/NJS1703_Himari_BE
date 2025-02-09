namespace HimariServer.API.Models
{
    public class BaseResponseModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
    }
}
