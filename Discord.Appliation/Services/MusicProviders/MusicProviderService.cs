using Discord.Appliation.Services.MusicProviders.Types;

namespace Discord.Appliation.Services.MusicProviders
{
    public class MusicProviderService : IMusicProviderService
    {
        private readonly IMusicProviderFactory _musicProviderFactory;
        private readonly IFileService _fileService;

        public MusicProviderService(IMusicProviderFactory musicProviderFactory,
            IFileService fileService)
        {
            _musicProviderFactory = musicProviderFactory;
            _fileService = fileService;
        }

        public async Task<SaveTrackResult> SaveTrack(string info)
        {
            var provider = _musicProviderFactory.Create(info);
            var path = $"{_fileService.GetCurrentDirectory()}{Consts.ProcessingDir}";

            var filePath = await provider.SaveTrack(path, info);

            return new SaveTrackResult
            {
                Path = filePath
            };
        }
    }

    public interface IMusicProviderService
    {
        Task<SaveTrackResult> SaveTrack(string info);
    }
}
