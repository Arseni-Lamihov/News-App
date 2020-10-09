using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using news_server.Infrastructure.Extensions;
using news_server.Infrastructure.Filter;
using news_server.Infrastructure.MiddleWare;

namespace news_server
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
            =>
            Configuration = configuration;    
       
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDataBaseContext(Configuration)
                .AddIdentityService()
                .AddJwtAuthentication(Configuration)
                .AddAppServices()
                .AddSwaggerService()
                .AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {               
                app.UseHsts();
            }
            app
                .UseSwaggerMW()
                .UseCorsMW()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseLastActivity()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }
}
