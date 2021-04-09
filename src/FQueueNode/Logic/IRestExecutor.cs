using FQueue.Logic;

namespace FQueueNode.Logic
{
    /// <summary>
    /// RestExecutor is to generate and validate input data for the NodeQueueManager to handle.
    /// </summary>
    public interface IRestExecutor
    {
        ExecutorResult<string> Dequeue(string queueName, int count, bool checkCount);
        ExecutorResult<string> Count(string queueName);
        ExecutorResult<string> Peek(string queueName);
        ExecutorResult<string> PeekTag(string queueName);
        ExecutorResult Enqueue(string queueName, string requestBody);
        ExecutorResult<string> Backup(string queueName, string filename);
        ExecutorResult<string> BackupAll(string folder);
    }
}