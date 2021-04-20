using System;
using System.Linq;
using FQueue.Configuration;
using FQueue.Context;
using FQueue.Logic;
using FQueue.Models;
using Newtonsoft.Json.Linq;

namespace FQueueNode.Logic
{
    /// <summary>
    /// RestExecutor is to generate and validate input data for the NodeQueueManager to handle.
    /// </summary>
    public class RestExecutor : IRestExecutor
    {
        private readonly IConfigurationReader _configurationReader;
        private readonly IQueueContextFactory _queueContextFactory;
        private readonly INodeQueueManager _nodeQueueManager;

        public RestExecutor(IConfigurationReader configurationReader, IQueueContextFactory queueContextFactory, INodeQueueManager nodeQueueManager)
        {
            _configurationReader = configurationReader;
            _queueContextFactory = queueContextFactory;
            _nodeQueueManager = nodeQueueManager;
        }

        protected virtual T LockLocallyWithContext<T>(string queueName, Func<QueueContext, T> action)
        {
            QueueContext context = _queueContextFactory.GetContext(queueName);

            lock (context)
            {
                return action(context);
            }
        }

        public LogicResult Dequeue(string queueName, int count, bool checkCount)
        {
            return LockLocallyWithContext(queueName, context => _nodeQueueManager.Dequeue(context, count, checkCount));
        }

        public LogicResult Count(string queueName)
        {
            return LockLocallyWithContext(queueName, context => _nodeQueueManager.Count(context));
        }

        public LogicResult Peek(string queueName)
        {
            return LockLocallyWithContext(queueName, context => _nodeQueueManager.Peek(context));
        }

        public LogicResult PeekTag(string queueName)
        {
            return LockLocallyWithContext(queueName, context => _nodeQueueManager.PeekTag(context));
        }

        public LogicResult Enqueue(string queueName, string requestBody)
        {
            return LockLocallyWithContext(queueName, context =>
            {
                if (String.IsNullOrWhiteSpace(requestBody))
                {
                    return new SuccessResult();
                }

                JToken token = JToken.Parse(requestBody);

                QueueEntry[] entries =
                    token.Type == JTokenType.Array
                        ? token.Children().Select(p => QueueEntry.FromRequestString(context, p.ToString(), _configurationReader.Configuration.Files.DataFileMaximumSizeB)).ToArray()
                        : new[] {QueueEntry.FromRequestString(context, token.ToString(), _configurationReader.Configuration.Files.DataFileMaximumSizeB)};

                return _nodeQueueManager.Enqueue(context, entries);
            });
        }

        public LogicResult Backup(string queueName, string filename)
        {
            return LockLocallyWithContext(queueName, context =>
            {
                if (String.IsNullOrWhiteSpace(filename))
                {
                    filename = context.GetDefaultBackupFile(_configurationReader.Configuration.Files.BackupFolder);
                }

                return _nodeQueueManager.Backup(context, filename);
            });
        }
    }
}