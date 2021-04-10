using System;
using FQueue.Rest.SwaggerAttributes;

namespace FQueue.Logic
{
    public class QueueNotFoundResult : LogicResult
    {
        public QueueNotFoundResult()
            : base(QueueNotFoundResponseAttribute.CODE)
        {
        }

        public override string DataToString()
        {
            return String.Empty;
        }
    }
}