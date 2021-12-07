using Discord.Appliation.Services;
using Discord.Appliation.Services.Audio;
using Discord.Commands;

namespace Discord.Appliation.Modules
{
    public class MusicModule : ModuleBase<SocketCommandContext>
    {
        private readonly IAudioService _audioService;
        private readonly IFileService _fileService;

        public MusicModule(IAudioService audioService,
            IFileService fileService)
        {
            _audioService = audioService;
            _fileService = fileService;
        }

        [Command("скажи", RunMode = RunMode.Async)]
        [Alias("с")]
        [Summary("Says a phrase")]
        public async Task Say([Remainder] string phrase)
        {
            var channel = (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null)
            {
                await ReplyAsync($"{Context.User.Mention}, ты должен быть в голосовом канале для того чтобы активировать команду");
            }

            try
            {
                await _audioService.Say(channel, phrase);
            }
            catch(FileNotFoundException ex)
            {
                await ReplyAsync($"{Context.User.Mention}, такой фразы не существует, попробуй 'список'");

            }
        }

        [Command("список")]
        [Summary("Prints phrase list")]
        public async Task List()
        {
            var path = _fileService.GetCurrentDirectory();
            var filePath = $"{path}{Consts.PhrasesDir}";

            var fileNames = _fileService.GetFileNames(filePath).Select(c => c.Split('\\').Last().Split('.')[0]);

            await ReplyAsync(string.Join(Environment.NewLine, fileNames));
        }

        [Command("играй", RunMode = RunMode.Async)]
        [Alias("и")]
        [Summary("Puts a track onto a queue")]
        public async Task Queue([Remainder] string info)
        {
            var channel = (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null)
            {
                await ReplyAsync($"{Context.User.Mention}, ты должен быть в голосовом канале для того чтобы активировать команду");
            }

            try
            {
                await _audioService.Queue(channel, info);
            }
            catch (NotSupportedException ex)
            {
                await ReplyAsync($"{Context.User.Mention}, данный тип провайдера не поддерживается");

            }
        }

        [Command("стоп", RunMode = RunMode.Async)]
        [Summary("Stops a music")]
        public async Task Stop()
        {
            var channel = (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null)
            {
                await ReplyAsync($"{Context.User.Mention}, ты должен быть в голосовом канале для того чтобы активировать команду");
            }

            await _audioService.Stop(channel);
        }

        [Command("скип", RunMode = RunMode.Async)]
        [Summary("Skips a track")]
        public async Task Skip()
        {
            var channel = (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null)
            {
                await ReplyAsync($"{Context.User.Mention}, ты должен быть в голосовом канале для того чтобы активировать команду");
            }

            await _audioService.Skip(channel);
        }
    }
}
