using HR_Management.Data.Repositories;
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using HR_Management.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace HR_Management.Extensions.Middleware
{
    public static class CustomizeMiddleware
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IPersonService, PersonService>();

            services.AddScoped<IEducationRepository, EducationRepository>();
            services.AddScoped<IEducationService, EducationService>();

            services.AddScoped<ICertificateRepository, CertificateRepository>();
            services.AddScoped<ICertificateService, CertificateService>();

            services.AddScoped<IWorkHistoryRepository, WorkHistoryRepository>();
            services.AddScoped<IWorkHistoryService, WorkHistoryService>();

            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ILocationService, LocationService>();

            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IProjectService, ProjectService>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<ICategoryPersonRepository, CategoryPersonRepository>();
            services.AddScoped<ICategoryPersonService, CategoryPersonService>();

            services.AddScoped<ITechnologyRepository, TechnologyRepository>();
            services.AddScoped<ITechnologyService, TechnologyService>();

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IInformationService, InformationService>();

            services.AddScoped<ITokenManagementService, TokenManagementService>();

            services.AddScoped<IImageService, ImageService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddCustomizeSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Human Resource Management for IT Company", Version = "v1" });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Human Resource Management for IT Company",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // Must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });
            });
        }

        public static void UseCustomizeSwagger(this IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Human Resource Management for IT Company");
                c.DocumentTitle = "Human Resource Management for IT Company";
            });
        }
    }
}
