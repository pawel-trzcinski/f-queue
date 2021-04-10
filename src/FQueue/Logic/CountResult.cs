using System.Globalization;

namespace FQueue.Logic
{
    public class CountResult : SuccessResult
    {
#warning TODO - unit tests
        private readonly int _count;

        public CountResult(int count)
        {
            _count = count;
        }

        public override string DataToString()
        {
            return _count.ToString(CultureInfo.InvariantCulture);
        }
    }
}