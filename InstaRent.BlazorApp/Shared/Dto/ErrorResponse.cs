namespace InstaRent.BlazorApp.Shared.Dto
{
    public class ErrorResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public Object Data { get; set; }
        public string ValidationErrors { get; set; }
    }

    public class ErrorResponseMessage
    {
        public ErrorResponse Error { get; set; }
    }
}
