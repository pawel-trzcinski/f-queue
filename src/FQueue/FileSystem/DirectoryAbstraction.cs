using System.IO;

namespace FQueue.FileSystem
{
    public class DirectoryAbstraction : IDirectoryAbstraction
    {
        public bool Exists(string path)
        {
            return Directory.Exists(path);
        }
    }
}