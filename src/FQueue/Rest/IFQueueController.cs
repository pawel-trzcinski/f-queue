using System;
using Microsoft.AspNetCore.Mvc;

namespace FQueue.Rest
{
    public interface IFQueueController : IDisposable
    {
        StatusCodeResult Test();
    }
}
