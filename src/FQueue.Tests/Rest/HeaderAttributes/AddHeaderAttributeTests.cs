using System.Collections.Generic;
using AutoFixture;
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
    public class AddHeaderAttributeTests
    {
        private class AddHeaderAttributeTester : AddHeaderAttribute
        {
            public AddHeaderAttributeTester(string name, string value) 
                : base(name, value)
            {
            }
        }

        [Test]
        [Repeat(5)]
        public void AttributeAddscCorrectHeader()
        {
            Fixture fixture = new Fixture();
            string name = fixture.Create<string>();
            string value = fixture.Create<string>();

            AddHeaderAttributeTester attribute = new AddHeaderAttributeTester(name, value);

            ActionContext actionContext = new ActionContext
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ControllerActionDescriptor()
            };
            ResultExecutingContext context = new ResultExecutingContext(actionContext, new List<IFilterMetadata>(), new StatusCodeResult(200), new TestController("XXX"));

            attribute.OnResultExecuting(context);

            Assert.IsTrue(actionContext.HttpContext.Response.Headers.ContainsKey(name));
            Assert.AreEqual(value, actionContext.HttpContext.Response.Headers[name][0]);
        }
    }
}