using System.IO;
using System.Text;

namespace FQueue.FileSystem
{
    public class FileAbstraction : IFileAbstraction
    {
        private static readonly Encoding _encoding = Encoding.UTF8;

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path, _encoding);
        }

        public void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents, _encoding);
        }
    }
}