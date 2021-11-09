using API.Controllers.Config;
using API.Extensions;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Information;
using Business.Services;
using Infrastructure.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API
{
    public class Startup
    {
        #region Constructor
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            // Calling response-message.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"responsemessage.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            configuration = builder.Build();

            Configuration = configuration;
        }
        #endregion

        #region Property
        public IConfiguration Configuration { get; }
        public static string WebHost { get; private set; }
        public static JwtConfig JwtConfig { get; private set; }
        #endregion

        #region Method
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                // Adds a custom error response factory when ModelState is invalid
                options.InvalidModelStateResponseFactory = InvalidResponseFactory.ProduceErrorResponse;
            });

            // Get the base URL of the application (http(s)://www.api.com) from the HTTP Request and Context.
            services.AddHttpContextAccessor();
            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });

            // Multipart body length limit
            services.Configure<FormOptions>(options =>
            {
                // Set the limit to 10 MB
                options.MultipartBodyLengthLimit = 10485760; // Bytes
            });

            // Configure JWT Bearer
            JwtConfig = Configuration.GetSection("JwtConfig").Get<JwtConfig>();
            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));

            // Mapping data from response-message.json
            services.Configure<ResponseMessage>(Configuration.GetSection(nameof(ResponseMessage)));

            // Mapping host information
            services.Configure<HostResource>(Configuration.GetSection("PathConfig"));

            services.AddJwtBearerAuthentication();
            services.AddCustomizeSwagger();
            services.AddDbContext<AppDbContext>(opts =>
            {
                opts.UseSqlServer(Configuration["ConnectionStrings:AppConnection"]);
            });
            services.AddDependencyInjection();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.DefaultModelsExpandDepth(-1); // Remove Schema on Swagger UI
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Human Resource Management for IT Company");
                    c.DocumentTitle = "Human Resource Management for IT Company";
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        #endregion
    }
}