using System.IO;

namespace FQueue.FileSystem
{
    public class DirectoryAbstraction : IDirectoryAbstraction
    {
#warning TODO - unit tests
        public bool Exists(string path)
        {
            return Directory.Exists(path);
        }
    }
}