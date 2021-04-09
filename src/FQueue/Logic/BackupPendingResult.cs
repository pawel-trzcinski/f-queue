using FQueue.Rest.SwaggerAttributes;

namespace FQueue.Logic
{
    public class BackupPendingResult<T> : ExecutorResult<T>
    {
        public BackupPendingResult()
            : base(BackupPendingResponseAttribute.CODE, default)
        {
        }
    }

    public class BackupPendingResult : ExecutorResult
    {
        public BackupPendingResult()
            : base(BackupPendingResponseAttribute.CODE)
        {
        }
    }
}