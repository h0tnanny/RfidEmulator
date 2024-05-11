
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using RfidEmulator.Api.Configurations;
using RfidEmulator.Api.ServiceExtensions;
using RfidEmulator.Api.Services;
using RfidEmulator.Repository;

namespace RfidEmulator.Api;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();
        
        var connectionString = builder.Configuration.GetConnectionString("postgres");

        if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException("Строчка подключения не указана!");
        
        // Add services to the container.
        builder.Services.AddDbContext<RepositoryContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        
        builder.Services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

        builder.Services.Configure<KafkaConfig>(builder.Configuration.GetSection("KafkaConfig"));
        builder.Services.Configure<PythonService>(builder.Configuration.GetSection("PythonService"));
        
        builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();
        builder.Services.AddSingleton<IEmulatorManager, EmulatorManager>();
        builder.Services.AddScoped<IReaderService, ReaderService>();
        builder.Services.AddSingleton<IOptimizationService, OptimizationService>();
        
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddMapster();
        builder.Services.AddHealthChecks();
        builder.Services.RegisterMapsterConfiguration();
        builder.Services.AddSignalR();
        builder.Services.AddSingleton<EmulatorHub>();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapHealthChecks("/healthCheck");
        app.UseHttpsRedirection();
        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.MapHub<EmulatorHub>("/emulatorHub");
        
        app.MapControllers();

        await app.RunAsync();
    }
}
