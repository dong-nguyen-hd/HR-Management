using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace HR_Management.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(cfg =>
            {
                cfg.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Human Resource Management",
                    Version = "v3",
                    Description = "Web API using for Human Resource Management in company.",
                    Contact = new OpenApiContact
                    {
                        Name = "Kim Young Ken",
                        Url = new Uri("https://github.com/dong-nguyen-hd")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "GNU",
                    },
                });

                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //cfg.IncludeXmlComments(xmlPath);
            });
            return services;
        }

        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger().UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Human Resource Management API");
                options.DocumentTitle = "Human Resource Management API";
            });
            return app;
        }
    }
}
