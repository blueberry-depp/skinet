﻿using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    // All of controllers and therefore all of the actions inside of those controllers are going to be making use of this action filter.
    // [ServiceFilter(typeof(LogUserActivity))] 
    // This is quite useful it saves us from manually checking to see if there's any validation errors.
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
    }
}
