using Discord.Appliation.Configurations;
using Discord.WebSocket;
using Microsoft.Extensions.Options;

namespace Discord.Appliation.Filters.Implementations
{
    public class PrefixFilter : IFilter
    {
        public Filter Type => Filter.Prefix;

        private readonly BotConfiguration _config;

        public PrefixFilter(IOptions<BotConfiguration> config)
        {
            _config = config.Value;
        }

        public Task<bool> Handle(SocketMessage message)
        {
            return Task.FromResult(message.Content[0] == _config.Prefix[0]);
        }
    }
}
