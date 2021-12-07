using Discord.Appliation.Configurations;
using Discord.Appliation.Filters;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace Discord.Appliation.Services
{
    public class DiscordService : IHostedService
    {
        private readonly ILogger _logger;

        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly IEnumerable<IFilter> _filters;
        private readonly IServiceProvider _serviceProvider;

        private readonly BotConfiguration _config;

        public DiscordService(ILogger logger,
            CommandService commandService,
            IServiceProvider serviceProvider,
            IEnumerable<IFilter> filters,
            IOptions<BotConfiguration> config
            )
        {
            _logger = logger;

            _commandService = commandService;
            _serviceProvider = serviceProvider;
            _filters = filters;
            _config = config.Value;

            _client = new DiscordSocketClient();
            _client.Log += Log;
            _client.MessageReceived += HandleMesage;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Started service");

            await _client.LoginAsync(TokenType.Bot, _config.Token);
            await _client.StartAsync();

            await _client.SetStatusAsync(UserStatus.Online);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Information("Stopped service");

            await _client.SetStatusAsync(UserStatus.Offline);
            await _client.StopAsync();
        }

        private Task Log(LogMessage msg)
        {
            _logger.Information(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task HandleMesage(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            var results = _filters.Select(c => c.Handle(message));
            await Task.WhenAll(results);

            if (results.Any(c => !c.Result))
            {
                return;
            }

            var context = new SocketCommandContext(_client, message);

            await _commandService.ExecuteAsync(context: context, argPos: 1, services: _serviceProvider);
        }
    }
}