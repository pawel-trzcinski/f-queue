using JetBrains.Annotations;

namespace FQueue.FileSystem.VersionFile
{
    public interface IWriteVersionFileCommand : ICommand
    {
        void SetInputData(string filename, [NotNull] VersionData versionData);
    }
}