using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Http;

namespace Arbor.App.Extensions.Http
{
    [UsedImplicitly]
    public class CustomLoggingFilter : IHttpMessageHandlerBuilderFilter
    {
        public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
        {
            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            return builder => { };
        }
    }
}