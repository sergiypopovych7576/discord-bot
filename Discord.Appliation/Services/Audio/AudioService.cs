using Discord.Appliation.Services.Audio.Types;
using Discord.Appliation.Services.MusicProviders;
using Discord.Audio;
using System.Collections.Concurrent;

namespace Discord.Appliation.Services.Audio
{
    public class AudioService : IAudioService
    {
        private readonly ConcurrentDictionary<ulong, AudioChannelnfo> _audioChannelInfos;

        private readonly IFileService _fileService;
        private readonly IAudioFileService _audioFileService;
        private readonly IMusicProviderService _musicProviderSerivce;

        public AudioService(IFileService fileService,
            IAudioFileService audioFileService,
            IMusicProviderService musicProviderSerivce)
        {
            _audioChannelInfos = new ConcurrentDictionary<ulong, AudioChannelnfo>();
            _fileService = fileService;
            _audioFileService = audioFileService;
            _musicProviderSerivce = musicProviderSerivce;
        }

        public async Task Queue(IVoiceChannel voiceChannel, string info)
        {
            AudioChannelnfo channelInfo = await GetChannelInfo(voiceChannel);

            var result = await _musicProviderSerivce.SaveTrack(info);
            var audioStream = await _audioFileService.Read(result.Path);

            channelInfo.Queue.Enqueue(new TrackInfo { Stream = audioStream });
            _audioChannelInfos[voiceChannel.Id] = channelInfo;

            if (!channelInfo.IsPlaying)
            {
                channelInfo.IsPlaying = true;
                _audioChannelInfos[voiceChannel.Id] = channelInfo;

                RunSongs(channelInfo);
            }
        }

        public async Task Say(IVoiceChannel voiceChannel, string phrase)
        {
            AudioChannelnfo channelInfo = await GetChannelInfo(voiceChannel);

            var path = _fileService.GetCurrentDirectory();
            var filePath = $"{path}{Consts.PhrasesDir}{phrase}.mp3";

            var fileExists = _fileService.CheckFileExists(filePath);
            if (!fileExists)
            {
                throw new FileNotFoundException();
            }

            var audioStream = await _audioFileService.Read(filePath);
            channelInfo.Queue.Enqueue(new TrackInfo { Stream = audioStream });
            _audioChannelInfos[voiceChannel.Id] = channelInfo;

            if (!channelInfo.IsPlaying)
            {
                channelInfo.IsPlaying = true;
                _audioChannelInfos[voiceChannel.Id] = channelInfo;

                RunSongs(channelInfo);
            }
        }

        public async Task Stop(IVoiceChannel voiceChannel)
        {
            AudioChannelnfo channelInfo = await GetChannelInfo(voiceChannel);
            channelInfo.Queue.Clear();
            channelInfo.IsPlaying = false;
            await channelInfo.VoiceStream.ClearAsync(CancellationToken.None);
            await channelInfo.VoiceStream.DisposeAsync();
            channelInfo.VoiceStream = null;

            _audioChannelInfos[voiceChannel.Id] = channelInfo;
        }

        public async Task Skip(IVoiceChannel voiceChannel)
        {
            AudioChannelnfo channelInfo = await GetChannelInfo(voiceChannel);
            channelInfo.IsPlaying = true;
            await channelInfo.VoiceStream.ClearAsync(CancellationToken.None);
            await channelInfo.VoiceStream.DisposeAsync();
            channelInfo.VoiceStream = channelInfo.Client.CreatePCMStream(AudioApplication.Mixed);

            _audioChannelInfos[voiceChannel.Id] = channelInfo;

            RunSongs(channelInfo);
        }

        private async Task RunSongs(AudioChannelnfo channelInfo)
        {
            while (true)
            {
                _audioChannelInfos.TryGetValue(channelInfo.VoiceChannelId, out channelInfo);

                if (channelInfo.Queue.Any() || !channelInfo.IsPlaying)
                {
                    var nextTrack = channelInfo.Queue.Dequeue();

                    await nextTrack.Stream.CopyToAsync(channelInfo.VoiceStream);
                    await nextTrack.Stream.DisposeAsync();
                }
                else
                {
                    channelInfo.IsPlaying = false;
                    _audioChannelInfos[channelInfo.VoiceChannelId] = channelInfo;
                    break;
                }
            }
        }


        private async Task<AudioChannelnfo> GetChannelInfo(IVoiceChannel voiceChannel)
        {
            AudioChannelnfo channelInfo;
            _audioChannelInfos.TryGetValue(voiceChannel.Id, out channelInfo);
            if (channelInfo == null)
            {
                channelInfo = new AudioChannelnfo
                {
                    VoiceChannelId = voiceChannel.Id,
                    Client = await voiceChannel.ConnectAsync(true),
                    Queue = new Queue<TrackInfo>(),
                    IsPlaying = false
                };

                var randomGreeting = await GetRandomGreetingPhrase();
                channelInfo.Queue.Enqueue(new TrackInfo { Stream = randomGreeting });
            }

            if(channelInfo.Client.ConnectionState == ConnectionState.Disconnected)
            {
                channelInfo.Client = await voiceChannel.ConnectAsync(true);
            }

            if (channelInfo.VoiceStream == null)
            {
                channelInfo.VoiceStream = channelInfo.Client.CreatePCMStream(AudioApplication.Mixed);
            }

            _audioChannelInfos[voiceChannel.Id] = channelInfo;

            return channelInfo;
        }

        private async Task<Stream> GetRandomGreetingPhrase()
        {
            var path = _fileService.GetCurrentDirectory();
            var greetingsPath = $"{path}{Consts.PhrasesDir}{Consts.GreetingsDir}";

            var files = _fileService.GetFileNames(greetingsPath);

            var random = new Random();
            var randRes = random.Next(0, files.Length);
            var phrase = files.ElementAt(randRes);

            var audioStream = await _audioFileService.Read(phrase);

            return audioStream;
        }
    }

    public interface IAudioService
    {
        Task Say(IVoiceChannel voiceChannel, string phrase);
        Task Queue(IVoiceChannel voiceChannel, string info);
        Task Stop(IVoiceChannel voiceChannel);
        Task Skip(IVoiceChannel voiceChannel);
    }
}
