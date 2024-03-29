using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using API.Helpers;
using API.Middleware;
using API.Extensions;
using Infrastructure.Identity;
using StackExchange.Redis;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddControllers();
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));
            });
            services.AddDbContext<StoreContext>(x => x.UseSqlite(_config.GetConnectionString("DefaultConnection")));

            // We're creating a new database here which is why we're specifying a separate service for this. We're having a completely
            // separate database for identity it's going to be a physical contact boundary between our own application database and the identity database.
            services.AddDbContext<AppIdentityDbContext>(x => x.UseSqlite(_config.GetConnectionString("IdentityConnection")));

            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                // true: ignore any unknown configuration.
                var configuration = ConfigurationOptions.Parse(_config.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });
            services.AddAplicationServices();
            services.AddIdentityServices(_config);
            services.AddSwaggerDocumentation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // This is for adding middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Regardless of which we're running in, we're just going to use our middleware.
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseSwaggerDocumentation();

            if (env.IsDevelopment())
            {

            }

            // In the event that request comes into API server but we don't have an end point that matches that particular request
            // then we're going to hit this middleware and it's going to redirect to ErrorController and pass in the
            // status code and in ErrorController in that particular route we're going to return new object's
            // results along with the status code which is going to result in this case in a 404 not found response.  
            // "{0}": the placeholder which represents the status code.
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            // This comes directly before the middleware to use authorization, if we add it afterwards then we're going to have a problem.
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
