using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorServerApp_Chess.Data;
using Microsoft.AspNetCore.ResponseCompression;
using BlazorServerApp_Chess.Hubs;
using DataAccessLibrary;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.SessionStorage;
using BlazorServerApp_Chess.Services;

namespace BlazorServerApp_Chess
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
                       
            services.AddTransient<ISQLDataAccess, SQLDataAccess>();
            services.AddTransient<IUserData, UserData>();
            services.AddScoped<UserService>();

            services.AddBlazoredSessionStorage();

            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

            services.AddSingleton<IConnectionManager, ConnectionManager>();

            services.AddSignalR();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCaching();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<ChatHub>("/chathub");
            //    routes.MapHub<GameHub>("/gamehub");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapHub<ChatHub>("/chathub");
                endpoints.MapHub<GameHub>("/gamehub");

            });

            
        }
    }
}
