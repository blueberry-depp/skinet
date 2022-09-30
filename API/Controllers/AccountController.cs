using System.Security.Claims;
using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    // And these are going to be the two managers that's going to control ability to register with the user and to sign in an existing user as well.
    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet]
    // Return UserDto, get the currently logged in user
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        // Technically because we're using an API we never actually logged in to the API we don't have that persistent connection to the API 
        // and our method of saying we're logged in is bypassing the token to the API for the request. And when get current user
        // this means that our client is going to send a token saying hey give me the current user based on the fact that they store a
        // token and when they restart their application then that process is going to fetch this particular method and just return the user details in
        // user DTO.

        // We need to get the email address out of the HttpContext, we have access to the http context purely by nature of the
        // fact we're inside a controller.
        // User: we can't guarantee that we've got this so give it optional and we look for the claims inside this user object and remember
        // when we created the token, we specified claims that we passed inside that token. So each token has a list of claims inside it and we'll
        // let the question mark because once again we can't guarantee this. This just gives us the email in this case let's say Bob@test.com.
        //var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        
        var user = await _userManager.FindByEmailFromClaimPrincipal(HttpContext.User);
        
        return new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            DisplayName = user.DisplayName
        };
    }

    // We've already seen that our API is going to reject any attempts to add a user with the same email that already exists inside our
    // user database but that's fine. This is not for our server. This is something that will allow our client to they actually attempt
    // to register the user. And this will be useful for async validation on the client side,
    // this is a method that's going to be used to check before they try and register a user to see if the email address has been taken.
    // FromQuery: get the string of the email from query so we'll just give the API a hint.
    [HttpGet("emailexists")]
    public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
    {
        // If this is not equal to null, this is going to return true. And the email address does exist and
        // this is just a helper method that we can provide to our clients so they can do asynchronous validation on the client side.
        return await _userManager.FindByEmailAsync(email) != null;
    }

    // We authorize against. Because we're getting our user and will simply run into an error if we don't ask for authentication
    // and yet try and get a user out of the HttpContext because it won't be available, we authorize we're going to need
    // to get to the email address out of the HttpContext.
    [Authorize]
    [HttpGet("address")]
    public async Task<ActionResult<AddressDto>> GetUserAddress()
    {
        // var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        // Inside the constructor we do not have HttpContext. It's actually null when we construct our controller,
        // so we only have access to HttpContext a little bit later on when we're inside our controller itself and it's already been created,
        // so we simply don't have HttpContext inside our constructors, we do have to do HttpContext insides the methods in the controller,
        // but what we actually want is ClaimPrincipal, because we have to use the HttpContext when we're inside this method(GetUserAddress)
        // and pass the HttpContext and the user to extension method.

        // We don't have include method inside UserManager so we can extend UserManager functionality to allow us to include some things
        // such as the address. 
        // Get the user address.
        var user = await _userManager.FindByUserClaimPrincipalWithAddressAsync(HttpContext.User);
        
        // We got the issue object cycle was detected. So we must use AddressDto to solve this issue.
        return _mapper.Map<Address, AddressDto>(user.Address);
    }


    [Authorize]
    [HttpPut("address")]
    // Technically speaking when we updating the address or creating a new address for a user, we updating the user because
    // the address is just the navigation property on a user object.
    public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
    {
        var user = await _userManager.FindByUserClaimPrincipalWithAddressAsync(HttpContext.User);
        
        // We can utilize automapper to update the properties in our user address, take the AddressDto/paramater/the body of request and map into Address.
        user.Address = _mapper.Map<AddressDto, Address>(address);

        // Update user via the UserManager.
        var result = await _userManager.UpdateAsync(user);
        
        // Return result the other way and pass back updated user address.
        if (result.Succeeded) return Ok(_mapper.Map<Address, AddressDto>(user.Address));

        return BadRequest("Problem updating the user");
    }
    

    [HttpPost("login")]
    // We're not going to return the full app user object because this is going to contain all of the properties that we saw inside our appuser
    // table inside our identity database as too much information send back. And also we want to send different information back as well. So
    // we'll use UserDto that we're going to return from this method. We'll create another dto(loginDto) that contain login information.
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        // I order to log in we need a combination of user manager get the user from database and then we'll use our sign in manager
        // to check the user's password against what stored in database.
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        // If we do get null from this we don't find the user with that particular email address then we're going to return unauthorized,
        // we'll use ApiResponse that we created earlier. So we get that consistent response being sent back to client.
        if (user == null) return Unauthorized(new ApiResponse(401));
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        
        // Check to see if result has succeeded.    
        if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
        
        return new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            DisplayName = user.DisplayName
        };
    }


    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            // set the username here because this required field inside identity and set this to email as well just say that we fulfill the requirements 
            // for identity
            UserName = registerDto.Email,
        };
            
        // This both create a user and saves the changes into the database.
        var result = await _userManager.CreateAsync(user, registerDto.Password);
        
        // Check the results once again because this creates async method returns an identity results, this will give us a succeeded flag
        // as well that we can check for. 
        // Note: by default identity is going to require a complex password. It is something that can be configured and changed.
        // 400: they're not actually doing anything in this case that requires them to authenticate we're just trying to create a user.
        if (!result.Succeeded) return BadRequest(new ApiResponse(400));

        return new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            DisplayName = user.DisplayName
        };
    }


}