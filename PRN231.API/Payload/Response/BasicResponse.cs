namespace PRN231.API.Payload.Response;

public class BasicResponse
{
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public object? Data { get; set; }
}