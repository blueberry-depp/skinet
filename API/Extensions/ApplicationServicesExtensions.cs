using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using API.Errors;
using Infrastructure.Services;

namespace API.Extensions
{
    // When we create extension method, the class itself must be static.
    public static class ApplicationServicesExtensions
    {
        // To use or extend the IServiceCollection that we're going to be returning, we need to use 'this' keyword.
        public static IServiceCollection AddAplicationServices(this IServiceCollection services/*, IConfiguration config*/)
        {
            services.AddScoped<ITokenService, TokenService>();
            // AddScoped: When request is finished then it disposes of both contoller and the repository
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IBasketRepository, BasketRepository>();

            // Setup for generic repository.
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));

            // Configure something related to controllers.
            services.Configure<ApiBehaviorOptions>(options =>
            {
                // ActionContext: because inside the action context is where we can get ModelState errors and that's
                // what our API attribute is using to populate any errors that are related to validation and add them into
                // a model state dictionary.
                options.InvalidModelStateResponseFactory = ActionContext =>
                {
                    // ModelState: is a dictionary type of object.
                    // And what we want to do is go inside this ModelState and extract the errors if there are 
                    // any and populates the error messages into an array and that's the array will pass into 
                    // ApiValidationErrorResponse() into the Errors property.
                    var errors = ActionContext.ModelState
                                // Check to see if there's any errors in here.
                                .Where(e => e.Value.Errors.Count > 0)
                                // Because this is a dictionary type of objects we're going to want to select all of the errors.
                                .SelectMany(x => x.Value.Errors)
                                // Select just the error messages and populate this into an array.
                                .Select(x => x.ErrorMessage);

                    // Initialize this class with the errors, 
                    var errorResponse = new ApiValidationErrorResponse
                    {
                        // and populate errors property inside this class.
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            //// AddSingleton: when instantiated is created and then it doesn't stop until our application stops. 
            //services.AddSingleton<PresenceTracker>();
            //// When we strongly type the key or configuration in this way use 'Configure'.
            //services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            //// AddScoped: when instantiated is created and then it stop when our http request are finished(suit for http request).
            //services.AddScoped<ITokenService, TokenService>();
            //services.AddScoped<IPhotoService, PhotoService>();
            //// When we inject UnitOfWork into a controller then it's going to have an instance,
            //// they're going to be using the data context that's injected into the unit of work. And that's the idea we pass that same
            //// instance, the single instance to each of our repositories.
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<LogUserActivity>();

            //// We only have a single project, so we only have a single assembly of where this can be created, so we use typeof(),
            //// this is enough for Automapper to go ahead and find those profiles.
            //services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            //services.AddDbContext<DataContext>(options =>
            //{
            //    //options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            //    // For heroku. 
            //    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            //    string connStr;

            //    // Depending on if in development or production, use either Heroku-provided
            //    // connection string, or development connection string from env var.
            //    if (env == "Development")
            //    {
            //        // Use connection string from file.
            //        connStr = config.GetConnectionString("DefaultConnection");
            //    }
            //    else
            //    {
            //        // Use connection string provided at runtime by Heroku.
            //        var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            //        // Parse connection URL to connection string for Npgsql
            //        connUrl = connUrl.Replace("postgres://", string.Empty);
            //        var pgUserPass = connUrl.Split("@")[0];
            //        var pgHostPortDb = connUrl.Split("@")[1];
            //        var pgHostPort = pgHostPortDb.Split("/")[0];
            //        var pgDb = pgHostPortDb.Split("/")[1];
            //        var pgUser = pgUserPass.Split(":")[0];
            //        var pgPass = pgUserPass.Split(":")[1];
            //        var pgHost = pgHostPort.Split(":")[0];
            //        var pgPort = pgHostPort.Split(":")[1];

            //        connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};SSL Mode=Require;TrustServerCertificate=True";
            //    }

            //    // Whether the connection string came from the local development configuration file
            //    // or from the environment variable from Heroku, use it to set up your DbContext.
            //    options.UseNpgsql(connStr);
            //});

            return services;
        }

    }
}
