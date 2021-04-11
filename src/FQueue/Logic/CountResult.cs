using System.Globalization;

namespace FQueue.Logic
{
    public class CountResult : SuccessResult
    {
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