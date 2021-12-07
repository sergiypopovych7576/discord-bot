using Discord.Appliation.Services.MusicProviders.Types;

namespace Discord.Appliation.Services.MusicProviders
{
    public interface IMusicProvider
    {
        public MusicProvider Provider { get; }

        Task<string> SaveTrack(string path, string info);
    }
}
