using System.IO;
using System.Text;

namespace FQueue.FileSystem
{
    public class FileAbstraction : IFileAbstraction
    {
#warning TODO - unit tests
        private static readonly Encoding ENCODING = Encoding.UTF8;

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path, ENCODING);
        }

        public void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents, ENCODING);
        }
    }
}