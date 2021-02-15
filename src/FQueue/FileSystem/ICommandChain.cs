namespace FQueue.FileSystem
{
    public interface ICommandChain : ICommand
    {
        void Add(ICommand command);
    }
}