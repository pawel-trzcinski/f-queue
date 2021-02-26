namespace FQueue.FileSystem
{
    public interface IDirectoryAbstraction
    {
        bool Exists(string path);
    }
}