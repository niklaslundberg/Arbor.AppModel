using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Http;

namespace Arbor.AppModel.Http
{
    [UsedImplicitly]
    public class CustomLoggingFilter : IHttpMessageHandlerBuilderFilter
    {
        public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
        {
            ArgumentNullException.ThrowIfNull(next);

            return _ => { };
        }
    }
}