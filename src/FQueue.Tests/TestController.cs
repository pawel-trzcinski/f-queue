using FQueue.Rest;
using Microsoft.AspNetCore.Mvc;

namespace FQueue.Tests
{
    [Route(Engine.FQUEUE + "/testC")]
    public class TestController : Controller, IFQueueController
    {
        private readonly string _stringToReturn;

        public ControllerContext Context
        {
            set => ControllerContext = value;
        }

        public TestController(string stringToReturn)
        {
            _stringToReturn = stringToReturn;
        }

        [HttpGet("testM")]
        public string Get()
        {
            return _stringToReturn;
        }

        public StatusCodeResult Test()
        {
            throw new System.NotSupportedException();
        }
    }
}