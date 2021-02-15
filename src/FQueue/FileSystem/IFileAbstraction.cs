namespace FQueue.FileSystem
{
    public interface IFileAbstraction
    {
        string ReadAllText(string path);
        void WriteAllText(string path, string contents);
    }
}