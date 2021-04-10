using System;
using FQueue.Rest.SwaggerAttributes;

namespace FQueue.Logic
{
    public class SuccessResult : LogicResult
    {
        public SuccessResult()
            : base(SuccessResponseAttribute.CODE)
        {
        }

        public override string DataToString()
        {
            return String.Empty;
        }
    }
}