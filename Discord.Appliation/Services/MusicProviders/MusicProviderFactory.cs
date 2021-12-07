using Discord.Appliation.Services.MusicProviders.Types;

namespace Discord.Appliation.Services.MusicProviders
{
    public class MusicProviderFactory : IMusicProviderFactory
    {
        private IEnumerable<IMusicProvider> _musicProviders;

        public MusicProviderFactory(IEnumerable<IMusicProvider> musicProviders)
        {
            _musicProviders = musicProviders;
        }

        public IMusicProvider Create(string info)
        {
            if (info.Contains("youtube") || info.Contains("youtu.be"))
            {
                return _musicProviders.First(c => c.Provider == MusicProvider.Youtube);
            }

            throw new NotImplementedException();
        }
    }

    public interface IMusicProviderFactory
    {
        IMusicProvider Create(string info);
    }
}
