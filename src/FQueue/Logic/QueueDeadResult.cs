using System;
using FQueue.Rest.SwaggerAttributes;

namespace FQueue.Logic
{
    public class QueueDeadResult : LogicResult
    {
        public QueueDeadResult()
            : base(QueueDeadResponseAttribute.CODE)
        {
        }

        public override string DataToString()
        {
            return String.Empty;
        }
    }
}