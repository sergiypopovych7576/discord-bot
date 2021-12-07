using Discord.Audio;

namespace Discord.Appliation.Services.Audio.Types
{
    public class AudioChannelnfo
    {
        public ulong VoiceChannelId { get; set; }
        public IAudioClient Client { get; set; }
        public AudioOutStream VoiceStream { get; set; }
        public Queue<TrackInfo> Queue { get; set; }
        public bool IsPlaying { get; set; }
    }
}
