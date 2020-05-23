﻿using System;

namespace Arbor.App.Extensions.ExtensionMethods
{
    public static class DisposeExtensions
    {
        public static void SafeDispose(this object? disposable)
        {
            if (disposable is null)
            {
                return;
            }

            if (!(disposable is IDisposable disposableItem))
            {
                return;
            }

            try
            {
                disposableItem.Dispose();
            }
            catch (ObjectDisposedException)
            {
                // ignore
            }
            catch (Exception)
            {
                // ignore
            }
        }
    }
}