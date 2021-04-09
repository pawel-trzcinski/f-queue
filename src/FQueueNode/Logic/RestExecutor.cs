using FQueue.Context;
using FQueue.Logic;

namespace FQueueNode.Logic
{
    /// <summary>
    /// RestExecutor is to generate and validate input data for the NodeQueueManager to handle.
    /// </summary>
    public class RestExecutor : IRestExecutor
    {
        private readonly IQueueContextFactory _queueContextFactory;
        private readonly INodeQueueManager _nodeQueueManager;

        public RestExecutor(IQueueContextFactory queueContextFactory, INodeQueueManager nodeQueueManager)
        {
            _queueContextFactory = queueContextFactory;
            _nodeQueueManager = nodeQueueManager;
        }

        public ExecutorResult<string> Dequeue(string queueName, int count, bool checkCount)
        {
#warning TODO
            throw new System.NotImplementedException();
        }

        public ExecutorResult<string> Count(string queueName)
        {
#warning TODO
            throw new System.NotImplementedException();
        }

        public ExecutorResult<string> Peek(string queueName)
        {
#warning TODO
            throw new System.NotImplementedException();
        }

        public ExecutorResult<string> PeekTag(string queueName)
        {
#warning TODO
            throw new System.NotImplementedException();
        }

        public ExecutorResult Enqueue(string queueName, string requestBody)
        {
#warning TODO
            throw new System.NotImplementedException();
        }

        public ExecutorResult<string> Backup(string queueName, string filename)
        {
#warning TODO
#warning TODO - jak bez nazwy pliku, to standardowa nazwa - zwraca ścieżkę, gdzie backup został zrobiony

            throw new System.NotImplementedException();
        }

        public ExecutorResult<string> BackupAll(string folder)
        {
#warning TODO
#warning TODO - jak bez nazwy foldery, to standardowa nazwa - zwraca ścieżkę, gdzie backupy zostały zrobione

            throw new System.NotImplementedException();
        }
    }
}