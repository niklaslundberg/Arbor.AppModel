using System;
using System.Linq;
using JetBrains.Annotations;

namespace Arbor.AppModel.ExtensionMethods
{
    public static class HexStringExtensions
    {
        public static byte[] FromHexToByteArray(this string hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(hex));
            }

            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }

        public static string FromByteArrayToHexString(this byte[] bytes)
        {
            ArgumentNullException.ThrowIfNull(bytes);

            return string.Concat(bytes.Select(b => b.ToString("X2")));
        }
    }
}