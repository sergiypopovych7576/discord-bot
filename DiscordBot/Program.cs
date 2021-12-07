using Discord.Appliation.Configurations;
using Discord.Appliation.Filters;
using Discord.Appliation.Filters.Implementations;
using Discord.Appliation.Services;
using Discord.Appliation.Services.Audio;
using Discord.Appliation.Services.MusicProviders;
using DiscordBot.Console.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Reflection;

await Host.CreateDefaultBuilder(args)
    .UseConsoleLifetime()
    .UseContentRoot(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
    .UseSerilog((hostingContext, services, loggerConfiguration) => loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console())
    .ConfigureServices((context, services) => ConfigureServices(context.Configuration, services))
    .RunConsoleAsync();

static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
{
    services.AddHostedService<DiscordService>();

    services.AddSingleton<IFileService, FileService>();
    services.AddSingleton<IAudioService, AudioService>();
    services.AddSingleton<IAudioFileService, AudioFileService>();
    services.AddSingleton<IMusicProviderService, MusicProviderService>();
    services.AddSingleton<IMusicProviderFactory, MusicProviderFactory>();

    services.AddSingleton<IMusicProvider, YoutubeMusicProvider>();

    services.AddSingleton<IFilter, PrefixFilter>();
    services.AddSingleton<IFilter, BotFilter>();

    services.RegisterDiscordCommands();

    services.Configure<BotConfiguration>(configuration.GetSection("Bot"));
}