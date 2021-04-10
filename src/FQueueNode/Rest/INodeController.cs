using FQueue.Models;
using FQueue.Rest;

namespace FQueueNode.Rest
{
    public interface INodeController : IFQueueController
    {
        string Dequeue(string queueName, int count, bool checkCount);
        string Count(string queueName);
        string Peek(string queueName);
        string PeekTag(string queueName);
        void Enqueue(string queueName, TagObject entry);
        string Backup(string queueName, string filename);
    }
}