using System;
using FQueue.Rest.SwaggerAttributes;

namespace FQueue.Logic
{
    public class MaintenancePendingResult : LogicResult
    {
        public MaintenancePendingResult()
            : base(MaintenancePendingResponseAttribute.CODE)
        {
        }

        public override string DataToString()
        {
            return String.Empty;
        }
    }
}