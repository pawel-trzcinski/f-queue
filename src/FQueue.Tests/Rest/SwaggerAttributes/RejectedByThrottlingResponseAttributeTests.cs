﻿using FQueue.Rest.SwaggerAttributes;
using NUnit.Framework;

namespace FQueue.Tests.Rest.SwaggerAttributes
{
    [TestFixture]
    public class RejectedByThrottlingResponseAttributeTests : SwaggerResponseAttributeTests
    {
        [Test]
        public void BasicCodeAndDescriptionAssert()
        {
            BasicCodeAndDescriptionAssert(new RejectedByThrottlingResponseAttribute());
        }
    }
}