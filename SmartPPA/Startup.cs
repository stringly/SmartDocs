using Google.Cloud.Diagnostics.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartPPA.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace SmartPPA
{
    public class Startup
    {
        IConfigurationRoot Configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">An <see cref="IHostingEnvironment"/> object.</param>
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json").Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGoogleExceptionLogging(options => {
                options.ProjectId = "smartdocs-219705";
                options.ServiceName = "SmartDocs";
                options.Version = "0.01";
            });
            services.AddDbContext<DocumentContext>(options => options.UseSqlServer(Configuration["Data:SmartDocuments:ConnectionString"]));
            services.AddTransient<IDocumentRepository, SmartDocumentRepository>();
            services.AddMvc();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider service)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseGoogleExceptionLogging();
            }
            app.UseGoogleExceptionLogging();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            SeedData.EnsurePopulated(service.GetRequiredService<DocumentContext>());
        }
    }
}
