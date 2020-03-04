using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Arbor.App.Extensions
{
    public static class ExceptionExtensions
    {
        public static bool IsFatal(this Exception ex)
        {
            if (ex == null)
            {
                return false;
            }

            return
                ex is OutOfMemoryException
                || ex is AccessViolationException
                || ex is AppDomainUnloadedException
                || ex is StackOverflowException
                || ex is ThreadAbortException
                || ex is SEHException;
        }

        public static string ThrowIfNullOrWhiteSpace(
            [NotNullIfNotNull("text")] this string? text,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new InvalidOperationException(
                    $"Value is null or empty {memberName} {sourceFilePath} {sourceLineNumber}".Trim());
            }

            return text;
        }
    }
}