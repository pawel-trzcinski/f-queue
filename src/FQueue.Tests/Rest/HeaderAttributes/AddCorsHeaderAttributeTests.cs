using System.Collections.Generic;
using FQueue.Rest.HeaderAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;

namespace FQueue.Tests.Rest.HeaderAttributes
{
    [TestFixture]
    public class AddCorsHeaderAttributeTests
    {
        [Test]
        public void AttributeAddscCorrectHeader()
        {
            AddCorsHeaderAttribute attribute = new AddCorsHeaderAttribute();

            ActionContext actionContext = new ActionContext
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ControllerActionDescriptor()
            };
            ResultExecutingContext context = new ResultExecutingContext(actionContext, new List<IFilterMetadata>(), new StatusCodeResult(200), new TestController("XXX"));

            attribute.OnResultExecuting(context);

            Assert.IsTrue(actionContext.HttpContext.Response.Headers.ContainsKey(AddCorsHeaderAttribute.ACCESS_CONTROL_ALLOW_ORIGIN));
            Assert.AreEqual("*", actionContext.HttpContext.Response.Headers[AddCorsHeaderAttribute.ACCESS_CONTROL_ALLOW_ORIGIN][0]);
        }
    }
}