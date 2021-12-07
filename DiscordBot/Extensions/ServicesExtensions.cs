using Discord.Appliation.Modules;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot.Console.Extensions
{
    public static class ServicesExtensions
    {
        public static void RegisterDiscordCommands(this IServiceCollection services)
        {
            var commandsService = new CommandService();
            services.AddSingleton(commandsService);

            var provider = services.BuildServiceProvider();

            commandsService.AddModulesAsync(assembly: typeof(MusicModule).Assembly, services: provider).GetAwaiter().GetResult();
        }
    }
}
