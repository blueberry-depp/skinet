namespace API.Errors
{
    public class ApiException : ApiResponse
    {
        // We make constructor to make it easy to use.
        // Exception always 500 in status code.
        public ApiException(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }

        public string Details { get; set; }


    }
}
