using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Thunder.Blog
{
    public class Startup
    {
        private static Exception _exception;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                throw new InvalidOperationException("This doesn't work!");
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
