namespace FQueue.QueueLockContext
{
    public interface ILockContextFactory
    {
        ILockContext CreateLockContext(string queueName);
    }
}