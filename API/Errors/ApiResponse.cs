namespace API.Errors
{
    public class ApiResponse
    {
        // We make constructor to make it easy to use.
        // Exception always 500 in status code.
        // The message set equal to null because we may not have a message that's attached to the response. And in that case
        // we'll replace it with one of our own.

        public ApiResponse(int statusCode, string message = null) 
        {
            StatusCode = statusCode;
            // Null coalescing operator(??), it basically means if message is null then execute what's to the right of these question marks.
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have made",
                401 => "Authorized, you are not",
                404 => "Resource found, it was not",
                500 => "Errors are the path to the dark side. Errors lead to anger.  Anger leads to hate.  Hate leads to career change",
                _ => null

            };   
        }

      
    }
}
