using AutoMapper;
using HR_Management.Data;
using HR_Management.Data.Contexts;
using HR_Management.Data.Repositories;
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using HR_Management.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HR_Management
{
    public class Startup
    {
        private IConfiguration Configuration { get; set; }

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Use SQL Server
            services.AddDbContext<AppDbContext>(opts =>
            {
                opts.UseSqlServer(
                Configuration["ConnectionStrings:AppConnection"]);
            });

            // Use DI
            services.AddScoped<IEducationRespository, EducationRepository>();
            services.AddScoped<IEducationService, EducationService>();
            // Use AutoMapper
            services.AddAutoMapper(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Seed data
            SeedData.EnsurePopulated(app);
        }
    }
}
