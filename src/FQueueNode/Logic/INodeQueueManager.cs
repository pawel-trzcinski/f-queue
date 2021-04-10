using FQueue.Context;
using FQueue.Logic;
using FQueue.Models;

namespace FQueueNode.Logic
{
    /// <summary>
    /// Main point of node logic. It is responsible for executing REST queue requests.
    /// </summary>
    public interface INodeQueueManager
    {
        LogicResult Dequeue(QueueContext context, int count, bool checkCount);
        LogicResult Count(QueueContext context);
        LogicResult Peek(QueueContext context);
        LogicResult PeekTag(QueueContext context);
        LogicResult Enqueue(QueueContext context, QueueEntry[] entries);
        LogicResult Backup(QueueContext context, string filename);
    }
}