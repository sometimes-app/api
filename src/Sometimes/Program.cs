

using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Sometimes;
using Sometimes.Constants;
using Sometimes.Database;
using Sometimes.Models;
using Sometimes.Services;
using System;
using System.Configuration;
using Sometimes.Services.Interfaces;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Configuration.AddJsonFile("./ignore.json");
        builder.Services.AddSingleton<AppSettingsConfig>();
        builder.Services.Configure<SometimesDbInfo>(builder.Configuration.GetSection(Constants.AppSettings.DatabaseInfo));

        SetupMongo(builder);
        SetupServices(builder);

        var app = builder.Build();
        // comment out and build to generate openapi file
#if !DEBUG
        app.Urls.Add("http://host.docker.internal:5228");
#endif
#if DEBUG
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            var devHostUrl = builder.Configuration.GetValue<string>(Constants.AppSettings.DevHost);
            app.Urls.Add(devHostUrl);
            app.UseSwagger();
            app.UseSwaggerUI();
        }
#endif

        app.UseAuthorization();

        app.MapControllers();

        app.Services.GetService<AppSettingsConfig>();
        app.UseHttpLogging();

        app.Run();

    }

    /// <summary>
    /// Setups mongo connection for DIO
    /// </summary>
    private static void SetupMongo(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
    }

    /// <summary>
    /// Setup Helper Services
    /// </summary>
    private static void SetupServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IUserInfoService, UserInfoService>();
    }
}


