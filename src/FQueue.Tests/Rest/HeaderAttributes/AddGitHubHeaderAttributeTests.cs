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
    public class AddGitHubHeaderAttributeTests
    {
        [Test]
        public void AttributeAddscCorrectHeader()
        {
            AddGitHubHeaderAttribute attribute = new AddGitHubHeaderAttribute();

            ActionContext actionContext = new ActionContext
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ControllerActionDescriptor()
            };
            ResultExecutingContext context = new ResultExecutingContext(actionContext, new List<IFilterMetadata>(), new StatusCodeResult(200), new TestController("XXX"));

            attribute.OnResultExecuting(context);

            Assert.IsTrue(actionContext.HttpContext.Response.Headers.ContainsKey(AddGitHubHeaderAttribute.GIT_HUB));
            Assert.AreEqual(AddGitHubHeaderAttribute.GIT_HUB_ADDRESS, actionContext.HttpContext.Response.Headers[AddGitHubHeaderAttribute.GIT_HUB][0]);
        }
    }
}