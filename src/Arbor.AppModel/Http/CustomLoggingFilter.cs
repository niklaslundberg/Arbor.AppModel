﻿using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Http;

namespace Arbor.AppModel.Http
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

            return _ => { };
        }
    }
}