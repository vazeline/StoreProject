using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Store.Binders;
using Store.Domain;
using Store.Domain.Abstract;
using Store.Domain.Concrete;

namespace Store
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddSession();
            services.AddMvc(options =>
            {
                // add custom binder to beginning of collection
                options.ModelBinderProviders.Insert(0, new CartModelBinderProvider());
            });
            services.AddDbContext<EFDbContext>(options =>options.UseSqlServer(Configuration.GetConnectionString("Store")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
            }
            app.UseSession(new SessionOptions {IdleTimeout = TimeSpan.FromMinutes(30)});
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Content")),
                RequestPath = new PathString("/Content")
            });

            app.UseMvc(routes =>
            {                
                routes.MapRoute(null, "", new { Controller = "Product", action = "List", category = (string)null, page = 1 });
                routes.MapRoute(null, "Page{page}", new { Controller = "Product", action = "List", category = (string)null }, new { page = @"\d+" });
                routes.MapRoute(null, "{Category}", new { controller = "Product", action = "List", page = 1 });
                routes.MapRoute(null, "{Category}/Page{page}", defaults: new { Controller = "Product", action = "List" }, constraints: new { page = @"\d+" });

                routes.MapRoute(null, "{controller}/{action}");
            });
        }
    }
}
