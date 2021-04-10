using NUnit.Framework;
using Swashbuckle.AspNetCore.Annotations;

namespace FQueue.Tests.Rest.SwaggerAttributes
{
    [TestFixture]
    public class SwaggerResponseAttributeTests
    {
        protected static void BasicCodeAndDescriptionAssert(SwaggerResponseAttribute attribute, bool success = false)
        {
            if (success)
            {
                Assert.AreEqual(200, attribute.StatusCode);
            }
            else
            {
                Assert.AreNotEqual(200, attribute.StatusCode);
            }

            Assert.IsNotNull(attribute.Description);
            Assert.Greater(attribute.Description.Trim().Length, 0);
        }
    }
}