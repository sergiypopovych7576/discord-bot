namespace Discord.Appliation.Services
{
    public class FileService : IFileService
    {
        public string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }

        public string[] GetFileNames(string path)
        {
            return Directory.GetFiles(path);
        }

        public bool CheckDirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public bool CheckFileExists(string path)
        {
            return File.Exists(path);
        }

        public Task WriteAllBytes(string path, byte[] bytes)
        {
            return File.WriteAllBytesAsync(path, bytes);
        }
    }

    public interface IFileService
    {
        string GetCurrentDirectory();
        string[] GetFileNames(string path);
        bool CheckDirectoryExists(string path);
        void CreateDirectory(string path);
        void DeleteFile(string path);
        bool CheckFileExists(string path);
        Task WriteAllBytes(string path, byte[] bytes);
    }
}
