using FQueue.Rest.SwaggerAttributes;
using NUnit.Framework;

namespace FQueue.Tests.Rest.SwaggerAttributes
{
    [TestFixture]
    public class QueueNameParameterAttributeTests : SwaggerResponseAttributeTests
    {
        [Test]
        public void BasicDescriptionAssert()
        {
            QueueNameParameterAttribute attribute = new QueueNameParameterAttribute();
            Assert.IsNotNull(attribute.Description);
            Assert.Greater(attribute.Description.Trim().Length, 0);
        }
    }
}