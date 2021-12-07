using Discord.Appliation.Services.Audio;
using Discord.Appliation.Services.MusicProviders.Types;
using VideoLibrary;

namespace Discord.Appliation.Services.MusicProviders
{
    public class YoutubeMusicProvider : IMusicProvider
    {
        public MusicProvider Provider => MusicProvider.Youtube;

        private readonly IFileService _fileService;
        private readonly IAudioFileService _audioFileService;

        public YoutubeMusicProvider(IFileService fileService,
            IAudioFileService audioFileService)
        {
            _fileService = fileService;
            _audioFileService = audioFileService;
        }

        public async Task<string> SaveTrack(string path, string info)
        {
            var yotube = YouTube.Default;
            var video = await yotube.GetVideoAsync(info);

            if (!_fileService.CheckDirectoryExists(path))
            {
                _fileService.CreateDirectory(path);
            }

            var videoPath = $"{path}\\{video.FullName}";
            var musicPath = $"{path}\\{video.FullName}.mp3";

            if(_fileService.CheckFileExists(musicPath))
            {
                return musicPath;
            }

            var videoBytes = await video.GetBytesAsync();
            await _fileService.WriteAllBytes(videoPath, videoBytes);

            await _audioFileService.ConvertVideoToAudio(videoPath, musicPath);
            _fileService.DeleteFile(videoPath);

            return musicPath;
        }
    }
}
