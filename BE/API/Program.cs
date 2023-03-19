using API.Controllers.Config;
using API.Extensions;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Information;
using Business.Services;
using Business.Services.CronJob;
using Infrastructure.Contexts;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

    // Calling response-message.json
    builder.Configuration.AddJsonFile("responsemessage.json", optional: false, reloadOnChange: true);

    #region Add services to the container.
    builder.Services.AddControllers(options =>
    {
        // Adds a custom ModelBinderProviders
        options.ModelBinderProviders.Insert(0, new QueryStringModelBinderProvider());
    }).ConfigureApiBehaviorOptions(options =>
    {
        // Adds a custom error response factory when ModelState is invalid
        options.InvalidModelStateResponseFactory = InvalidResponseFactory.ProduceErrorResponse;
    });
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddCronJob<DeleteRefreshTokenJob>(c =>
    {
        c.TimeZoneInfo = TimeZoneInfo.Local;
        c.CronExpression = @"0 * * * *"; // Every hour
    });

    // Get the base URL of the application (http(s)://www.api.com) from the HTTP Request and Context.
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddSingleton<IUriService>(o =>
    {
        var accessor = o.GetRequiredService<IHttpContextAccessor>();
        var request = accessor.HttpContext?.Request;
        var uri = string.Concat(request?.Scheme, "://", request?.Host.ToUriComponent());
        return new UriService(uri);
    });

    // Multipart body length limit
    builder.Services.Configure<FormOptions>(options =>
    {
        // Set the limit to 10 MB
        options.MultipartBodyLengthLimit = 10485760; // Bytes
    });

    // Configure JWT Bearer
    builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();

    // Mapping data from response-message.json
    builder.Services.Configure<ResponseMessage>(builder.Configuration.GetSection(nameof(ResponseMessage)));

    // Mapping host information
    builder.Services.Configure<HostResource>(builder.Configuration.GetSection("PathConfig"));

    builder.Services.AddJwtBearerAuthentication();
    builder.Services.AddCustomizeSwagger();
    builder.Services.AddDbContext<AppDbContext>(opts =>
    {
        opts.UseSqlServer(builder.Configuration["ConnectionStrings:AppConnection"]);
    });
    builder.Services.AddDependencyInjection();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
    });
    #endregion

    #region Configure the HTTP request pipeline.
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.DefaultModelsExpandDepth(-1); // Remove Schema on Swagger UI
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Human Resource Management for IT Company");
            c.DocumentTitle = "Human Resource Management for IT Company";
        });
    }

    //app.UseHttpsRedirection();
    app.UseMiddleware<ErrorHandlerMiddleware>();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseCors("AllowAll");
    app.UseSerilogRequestLogging();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
    #endregion
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}