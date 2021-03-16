using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Refit;
using Subway.Status.Business.MappingProfiles;
using Subway.Status.Domain;
using Subway.Status.Integration.Contracts;
using Subway.Status.Repository;
using System;

namespace Subway.Status.Api
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Subway Status API", Version = "v1" });
            });

            // Inyección de dependencias
            services.AddScoped<Business.Contracts.ISubwayBusiness, Business.SubwayBusiness>();
            services.AddScoped(typeof(Repository.Contracts.IRepository<>), typeof(Repository<>));

            // Se declara el cliente de Refit
            services.AddRefitClient<ISubwaysApi>()
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(Configuration.GetSection(Constants.SubwayApiUrl).Value));

            // Se agrega la configuracion de AutoMapper
            services.AddAutoMapper(cfg => cfg.AddProfile<SubwayMappingProfile>());

            // Se indica el contexto de DB y su connectionString
            services.AddDbContext<SubwayContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Habilitamos el Cors para que no tenga restricciones
            services.AddCors(opt => 
                opt.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                }));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SubwayContext subwayContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Aplica las migrations pendientes y crea la base en caso que no exista
                subwayContext.Database.Migrate();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Subway Status API");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
