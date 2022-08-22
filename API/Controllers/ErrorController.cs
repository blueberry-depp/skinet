
using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // We override the routes that we get from BaseApiController.
    // code: status code
    [Route("errors/{code}")]
    // For enable swagger.
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }
    }
}
