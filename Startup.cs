using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopsyTurvyCakes.Models;

namespace TopsyTurvyCakes
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false).AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizeFolder("/Admin");  // restrict access
                options.Conventions.AuthorizeFolder("/Account");  // restrict access
                options.Conventions.AllowAnonymousToPage("/Account/Login"); // punch holes in access 
            }); // disable endpoint-routing needed to be added
            services.AddScoped<IRecipesService, RecipesService>();  // provide an instance of the recipe service class anytime an instance of the IRecipesService is requested
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(); // you need to tell .NET what the authentication scheme will be

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseRouting();       
            app.UseStaticFiles();   // which this the project will look in the wwwroot folder for static files and return them if the filname matches the request
            app.UseMvcWithDefaultRoute();
        }
    }
}
