﻿using System;
using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace FQueue.Rest.SwaggerAttributes
{
    public class SuccessResponseAttribute : SwaggerResponseAttribute
    {
#warning TODO - unit tests
        public SuccessResponseAttribute(string appendedDescription = null)
            : base((int) HttpStatusCode.OK)
        {
            Description = "Operation was executed successfully." + (String.IsNullOrEmpty(appendedDescription) ? String.Empty : (" " + appendedDescription));
        }
    }
}