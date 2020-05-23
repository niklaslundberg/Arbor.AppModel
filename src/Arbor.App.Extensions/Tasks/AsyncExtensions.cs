using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Arbor.App.Extensions.Tasks
{
    public static class AsyncExtensions
    {
        /// <summary>
        ///     Allows a cancellation token to be awaited.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static CancellationTokenAwaiter GetAwaiter(this CancellationToken cancellationToken) =>
            // return our special awaiter
            new CancellationTokenAwaiter {internalTCancellationToken = cancellationToken};

        /// <summary>
        ///     The awaiter for cancellation tokens.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public struct CancellationTokenAwaiter : ICriticalNotifyCompletion
        {
            public CancellationTokenAwaiter(CancellationToken cancellationToken) =>
                internalTCancellationToken = cancellationToken;

#pragma warning disable IDE1006 // Naming Styles
            internal CancellationToken internalTCancellationToken;
#pragma warning restore IDE1006 // Naming Styles

            public object GetResult()
            {
                // this is called by compiler generated methods when the
                // task has completed. Instead of returning a result, we
                // just throw an exception.
                if (IsCompleted)
                {
                    throw new OperationCanceledException();
                }

                throw new InvalidOperationException("The cancellation token has not yet been cancelled.");
            }

            // called by compiler generated/.net internals to check
            // if the task has completed.
            public bool IsCompleted => internalTCancellationToken.IsCancellationRequested;

            // The compiler will generate stuff that hooks in
            // here. We hook those methods directly into the
            // cancellation token.
            public void OnCompleted(Action continuation) =>
                internalTCancellationToken.Register(continuation);

            public void UnsafeOnCompleted(Action continuation) =>
                internalTCancellationToken.Register(continuation);
        }
    }
}