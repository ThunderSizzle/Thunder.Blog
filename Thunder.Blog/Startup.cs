using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElmahCore.Mvc;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Thunder.Blog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
           _configuration = configuration;
        }

        private static Exception _exception;
        private IConfiguration _configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddElmah();
                
                services.AddHangfire(x => x.UseSqlServerStorage(Constants.ConnectionStringNames.HangfireConnection));

                services.AddHangfireServer();
            }
            catch(Exception ex)
            {
                _exception = ex;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if(_exception != null)
            {
                RenderExceptionPage(app, _exception);
                return;
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireDashboard();


            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }

        private static void RenderExceptionPage(IApplicationBuilder app, Exception exception)
        {
            app.UseDeveloperExceptionPage();
            app.Run((context) =>
            {
                throw new Exception("Something went wrong while trying to start the application. See inner exception for details.", exception);
            });
        }
    }
}
