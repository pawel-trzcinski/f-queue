namespace FQueue.Context
{
    public interface IQueueContextFactory
    {
        /// <summary>
        /// Creates context of given name. If the context with given name was created earlier, already existing context is created.
        /// Thias means that QueueCotntext is a singleton existing for a queue of given name.
        /// </summary>
        /// <returns>Context singleton</returns>
        QueueContext GetContext(string queueName);
    }
}