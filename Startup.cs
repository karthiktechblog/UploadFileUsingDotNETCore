using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace UploadFileUsingDotNETCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(
              options =>
              {
                  options.SwaggerDoc("v1", new OpenApiInfo()
                  {
                      Title = "Learn Smart Coding - Demo",
                      Version = "V1",
                      Description = "",
                      TermsOfService = new System.Uri("https://karthiktechblog.com/copyright"),
                      Contact = new OpenApiContact()
                      {
                          Name = "Learn Smart Coding",
                          Email = "karthiktechblog.com@gmail.com",
                          Url = new System.Uri("http://www.karthiktechblog.com")
                      },
                      License = new OpenApiLicense
                      {
                          Name = "Use under LICX",
                          Url = new System.Uri("https://karthiktechblog.com/copyright"),
                      }
                  });
              }
          );
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Karthiktechblog Restaurant API V1");
            });
        }
    }
}
