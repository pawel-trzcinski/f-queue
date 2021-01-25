namespace FQueue.FileSystem
{
    public interface IFileCommand
    {
        void Execute();
        void Rollback();
    }
}