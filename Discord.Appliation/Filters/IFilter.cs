using Discord.WebSocket;

namespace Discord.Appliation.Filters
{
    public interface IFilter
    {
        public Filter Type { get; }

        Task<bool> Handle(SocketMessage message);
    }
}
