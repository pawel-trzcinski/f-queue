using FQueue.Logic;

namespace FQueueNode.Logic
{
    /// <summary>
    /// RestExecutor is to generate and validate input data for the NodeQueueManager to handle.
    /// </summary>
    public interface IRestExecutor
    {
        LogicResult Dequeue(string queueName, int count, bool checkCount);
        LogicResult Count(string queueName);
        LogicResult Peek(string queueName);
        LogicResult PeekTag(string queueName);
        LogicResult Enqueue(string queueName, string requestBody);
        LogicResult Backup(string queueName, string filename);
    }
}