using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using API.Helpers;
using API.Middleware;
using API.Extensions;

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
            services.AddDbContext<StoreContext>(x => x.UseSqlite(_config.GetConnectionString("DefaultConnection")));
            services.AddAplicationServices();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
