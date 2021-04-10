using System.Net;

namespace FQueue.Logic
{
    public abstract class LogicResult
    {
#warning TODO - unit tests
        public HttpStatusCode Status { get; }

        public bool ShouldHaveData { get; }

        public bool IsOk => Status == HttpStatusCode.OK;

        protected LogicResult(HttpStatusCode status, bool shouldHaveData = true)
        {
            Status = status;
            ShouldHaveData = shouldHaveData;
        }

        public abstract string DataToString();
    }
}