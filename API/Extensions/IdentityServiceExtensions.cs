using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Extensions
{
    // When we create extension method, the class itself must be static
    public static class IdentityServiceExtensions
    {
        // To use or extend the IServiceCollection that we're going to be returning, we need to use 'this' keyword,
        // and inject config into our static method but we'll simply pass it as a parameter to IdentityServiceExtensions methods.
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            // AddIdentityCore is for SPA application.
            // opt is option.
            var builder = services.AddIdentityCore<AppUser>();
            builder = new IdentityBuilder(builder.UserType, builder.Services);
            builder.AddEntityFrameworkStores<AppIdentityDbContext>();
            builder.AddSignInManager<SignInManager<AppUser>>();

            services.AddAuthentication();
 
            // We're going to tell it what type of authentication we're using and how it needs to validate the token
            // that we're going to pass to clients.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // If we forget to add this then we might as well just leave anonymous authentication on and a user can
                        // send up any old token they want because we would never validate that the signing key is correct.
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                        // We're only going to trust tokens that are issued from this particular server but we need to tell
                        // it what's the valid issue where is and because we add this to the token when we generate the token we
                        // need this to be the same as our token issuer that we're going to add to our configurations.
                        ValidIssuer = config["Token:Key"],
                        // If we're adding a token issuer to our token then what we also need to do is actually validates the issuer otherwise again it will
                        // just accepts any issuer of any token.
                        ValidateIssuer = true,
                        ValidateAudience = false, // Our angular application
                    };
                });

            return services;
        }
    }
}