﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Arbor.AppModel.Tasks;

public static class AsyncExtensions
{
    /// <summary>
    ///     Allows a cancellation token to be awaited.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static CancellationTokenAwaiter GetAwaiter(this CancellationToken cancellationToken) =>
        // return our special awaiter
        new() { internalTCancellationToken = cancellationToken };

    /// <summary>
    ///     The awaiter for cancellation tokens.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct CancellationTokenAwaiter(CancellationToken cancellationToken) : ICriticalNotifyCompletion
    {
#pragma warning disable IDE1006 // Naming Styles
        // ReSharper disable once InconsistentNaming
        internal CancellationToken internalTCancellationToken = cancellationToken;
#pragma warning restore IDE1006 // Naming Styles

        public object GetResult()
        {
            if (IsCompleted)
            {
                return 0;
            }

            throw new InvalidOperationException("The cancellation token has not yet been cancelled.");
        }

        // called by compiler generated/.net internals to check
        // if the task has completed.
        public bool IsCompleted => internalTCancellationToken.IsCancellationRequested;

        // The compiler will generate stuff that hooks in
        // here. We hook those methods directly into the
        // cancellation token.
        public void OnCompleted(Action continuation) => internalTCancellationToken.Register(continuation);

        public void UnsafeOnCompleted(Action continuation) => internalTCancellationToken.Register(continuation);
    }
}