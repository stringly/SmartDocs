using Google.Cloud.Diagnostics.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartDocs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;


namespace SmartDocs
{
    /// <summary>
    /// Initializes the application environment
    /// </summary>
    public class Startup
    {
        IConfigurationRoot Configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartPPA.Startup"/> class.
        /// </summary>
        /// <param name="env">An <see cref="T:Microsoft.AspNetCore.Hosting.IHostingEnvironment"/> object.</param>
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json").Build();
        }

        /// <summary>
        /// Configures the application services.
        /// </summary>
        /// <param name="services">An <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection"/> object.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGoogleExceptionLogging(options => {
                options.ProjectId = "smartdocs-219705";
                options.ServiceName = "SmartDocs";
                options.Version = "0.01";
            });

            services.AddDbContext<SmartDocContext>(options => options.UseSqlServer(Configuration["Data:SmartDocuments:ConnectionString"]));            
            services.AddTransient<IDocumentRepository, SmartDocumentRepository>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<UserResolverService>();
            services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);
            services.AddMvc();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.  
        /// </summary>
        /// <param name="app">An <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder"/> object.</param>
        /// <param name="env">An <see cref="T:Microsoft.AspNetCore.Hosting.IHostingEnvironment"/> object.</param>
        /// <param name="service">An <see cref="T:System.IServiceProvider"/> object.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider service)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseGoogleExceptionLogging();
            }

            app.UseStatusCodePages();
            app.UseGoogleExceptionLogging();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Choices}/{id?}");
            });
        }
    }
}
