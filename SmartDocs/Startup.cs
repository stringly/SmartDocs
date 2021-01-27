using Google.Cloud.Diagnostics.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartDocs.Models;
using SmartDocs.Models.ViewModels;
using System;

namespace SmartDocs
{
    /// <summary>
    /// Initializes the application environment
    /// </summary>
    public class Startup
    {
        IConfigurationRoot Configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">An <see cref="IHostingEnvironment"/> object.</param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json",
                             optional: false,
                             reloadOnChange: true)
                .AddEnvironmentVariables();
            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>(); // required to load user secrets in Dev
            }                
            Configuration = builder.Build();
        }

        /// <summary>
        /// Configures the application services.
        /// </summary>
        /// <param name="services">An <see cref="IServiceCollection"/> object.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGoogleExceptionLogging(options =>
            {
                options.ProjectId = "smartdocs-219705";
                options.ServiceName = "SmartDocs";
                options.Version = "0.01";
            });

            services.AddDbContext<SmartDocContext>(options => options.UseSqlServer(Configuration["Data:SmartDocuments:ConnectionString"]));
            services.AddScoped<IClaimsTransformation, ClaimsLoader>();
            services.AddTransient<IDocumentRepository, SmartDocumentRepository>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();            
            services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);            
            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(0, new AwardTypeModelBinderProvider()); // custom binder for awards 
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.  
        /// </summary>
        /// <param name="app">An <see cref="IApplicationBuilder"/> object.</param>
        /// <param name="env">An <see cref="IHostingEnvironment"/> object.</param>
        /// <param name="service">An <see cref="IServiceProvider"/> object.</param>
        /// <param name="context">An instance of <see cref="SmartDocContext"/></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider service, SmartDocContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseGoogleExceptionLogging();
            }
            // comment out these lines if there is no need to push the old PPAs into the new DB
            DataInitializer initializer = new DataInitializer(context);
            initializer.SeedTemplates();       

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
