using FQueue.Rest;
using Microsoft.AspNetCore.Mvc;

namespace FQueueNode.Rest
{
    public interface INodeController : IFQueueController
    {
        string Dequeue(string queueName, int count);
        int Count(string queueName);
        string Peek(string queueName);
        string PeekTag(string queueName);
        StatusCodeResult Enqueue(string queueName, string entry);
        string Backup(string queueName, string filename);
    }
}