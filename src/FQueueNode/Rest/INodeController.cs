using FQueue.Models;
using FQueue.Rest;
using Microsoft.AspNetCore.Mvc;

namespace FQueueNode.Rest
{
    public interface INodeController : IFQueueController
    {
        string Dequeue(string queueName, int count, bool checkCount);
        string Count(string queueName);
        string Peek(string queueName);
        string PeekTag(string queueName);
        StatusCodeResult Enqueue(string queueName, TagObject entry);
        string Backup(string queueName, string filename);
        string BackupAll([FromQuery] string folder);
    }
}