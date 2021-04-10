using System;
using FQueue.Rest.SwaggerAttributes;

namespace FQueue.Logic
{
    public class BackupPendingResult : LogicResult
    {
        public BackupPendingResult()
            : base(BackupPendingResponseAttribute.CODE)
        {
        }

        public override string DataToString()
        {
            return String.Empty;
        }
    }
}