using Discord.WebSocket;

namespace Discord.Appliation.Filters.Implementations
{
    public class BotFilter : IFilter
    {
        public Filter Type => Filter.Bot;

        public Task<bool> Handle(SocketMessage message)
        {
            return Task.FromResult(!message.Author.IsBot);
        }
    }
}
