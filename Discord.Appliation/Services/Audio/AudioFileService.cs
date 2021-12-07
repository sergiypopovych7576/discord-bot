using System.Diagnostics;

namespace Discord.Appliation.Services.Audio
{
    public class AudioFileService : IAudioFileService
    {
        public async Task ConvertVideoToAudio(string videoPath, string audioPath)
        {
            var task = new Task(() =>
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "ffmpeg.exe",
                    Arguments = $"-y -i \"{videoPath}\" \"{audioPath}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                });

                process.WaitForExit();
                process.Close();
            });

            task.Start();
            await task;
        }

        public async Task<Stream> Read(string path)
        {
            var memoryStream = new MemoryStream();

            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });


            await process.StandardOutput.BaseStream.CopyToAsync(memoryStream);
            process.Close();

            memoryStream.Position = 0;
            return memoryStream;
        }

    }

    public interface IAudioFileService
    {
        Task<Stream> Read(string path);
        Task ConvertVideoToAudio(string videoPath, string audioPath);
    }
}
