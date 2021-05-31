using ChatApp.Database;
using ChatApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ChatApp.Hubs;

namespace ChatApp
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config) => _config = config;
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _config.GetConnectionString("DefaultConnection");
            services.AddMvc().AddMvcOptions(options => {
                options.EnableEndpointRouting = false;
            });

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<User, IdentityRole>(options => {
                options.Password.RequireDigit=false;
                options.Password.RequireLowercase=false;
                options.Password.RequireNonAlphanumeric=false;
                options.Password.RequireUppercase=false;
                options.Password.RequiredLength=6;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseEndpoints(routes => {
                routes.MapHub<ChatHub>("/chatHub");
            });
            app.UseMvcWithDefaultRoute();
        }
    }
}
