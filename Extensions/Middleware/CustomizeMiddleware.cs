using HR_Management.Data.Repositories;
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using HR_Management.Services;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddScoped<IImageService, ImageService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

    }
}
