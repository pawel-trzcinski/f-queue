namespace FQueue.DateTime
{
    public class DateTimeAbstraction : IDateTimeAbstraction
    {
        public System.DateTime UtcNow => System.DateTime.UtcNow;
    }
}