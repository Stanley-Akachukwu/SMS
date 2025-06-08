

namespace SMS.Common.Dtos
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public int StatusCode { get; set; }

        public static ApiResponse<T> Success(T data, int statusCode) =>
            new() { IsSuccess = true, Data = data, StatusCode = statusCode };

        public static ApiResponse<T> Failure(string errorMessage, int statusCode) =>
            new() { IsSuccess = false, ErrorMessage = errorMessage, StatusCode = statusCode };
    }
}
