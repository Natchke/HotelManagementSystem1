namespace HotelManagementSystem1
{
    public class ApiResponse
    {
        public string Message { get; set; }
        public object Data { get; set; }
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }

        public ApiResponse(string message, object data, int statusCode, bool isSuccess)
        {
            Message = message;
            Data = data;
            StatusCode = statusCode;
            IsSuccess = isSuccess;
        }
    }
}
