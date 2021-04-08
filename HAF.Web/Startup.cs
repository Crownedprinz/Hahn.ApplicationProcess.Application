using HAF.DAL.Commands;
using HAF.DAL.Queries;
using HAF.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HAF.Domain.QueryParameters;
using HAF.Domain.CommandParameters;
using Microsoft.OpenApi.Models;
using System.IO;
using Newtonsoft.Json.Converters;

namespace HAF.Web
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

            services.AddControllers().AddNewtonsoftJson(options =>
             options.SerializerSettings.Converters.Add(new StringEnumConverter()));
            // order is vital, this *must* be called *after* AddNewtonsoftJson()
            services.AddSwaggerGenNewtonsoftSupport(); ;
            services.AddScoped<IQueryAll, QueryEntities>();
            services.AddScoped<ICommand, AssetCommand>();
            services.AddScoped<IAssetService, AssetService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HAF.Web", Version = "v1" });
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "HAF.Web.xml");
                c.IncludeXmlComments(filePath);
                //c.DescribeAllEnumsAsStrings();
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HAF.Web v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors("MyPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
    