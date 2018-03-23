namespace WebAPI.RequestPipeline
{
    public class Error
    {
        public string Message { get; set; }
        public ErrorDetail[] Details { get; set; }
    }

    public class ErrorDetail
    {
        public string Target { get; set; }
        public string Message { get; set; }
    }
}