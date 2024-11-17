using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Arbor.AppModel.ExtensionMethods;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;

namespace Arbor.AppModel.Http
{
    public static class HttpExtensions
    {
        public static bool IsJsonResponse(this HttpResponseMessage response)
        {
            ArgumentNullException.ThrowIfNull(response);

            return ContentType.IsJson(response.Content.Headers.ContentType?.MediaType);
        }

        public static Task<T?> TrySendAndReadResponseJson<T>(this HttpClient client,
            HttpRequestMessage requestMessage,
            ILogger logger,
            CancellationToken cancellationToken) where T : class
        {
            ArgumentNullException.ThrowIfNull(client);

            ArgumentNullException.ThrowIfNull(requestMessage);

            return InternalTrySendAndReadResponseJson<T>(client, requestMessage, logger, cancellationToken);
        }

        private static async Task<T?> InternalTrySendAndReadResponseJson<T>(this HttpClient client,
            HttpRequestMessage requestMessage,
            ILogger logger,
            CancellationToken cancellationToken) where T : class
        {
            var response = await client.SendAsync(requestMessage, cancellationToken);

            return await TryReadJsonAs<T>(response, logger: logger);
        }

        public static Task<T?> TryReadJsonAs<T>(this HttpResponseMessage response,
            bool allowAnyResponseCode = false,
            ILogger? logger = null,
            JsonSerializer? serializer = null) where T : class
        {
            ArgumentNullException.ThrowIfNull(response);

            return InternalTryReadJsonAs<T>(response, allowAnyResponseCode, logger, serializer);
        }

        private static async Task<T?> InternalTryReadJsonAs<T>(this HttpResponseMessage response,
            bool allowAnyResponseCode = false,
            ILogger? logger = null,
            JsonSerializer? serializer = null) where T : class
        {
            logger ??= Logger.None!;

            if (!allowAnyResponseCode && !response.IsSuccessStatusCode)
            {
                logger.Error("Unexpected http response code {HttpResponseCode}", (int)response.StatusCode);

                return default;
            }

            if (!IsJsonResponse(response))
            {
                logger.Error("The response is not JSON format");
                return default;
            }

            await using var responseStream = await response.Content.ReadAsStreamAsync();

            using var streamReader = new StreamReader(responseStream);
            using var jsonTextReader = new JsonTextReader(streamReader);

            serializer ??= new JsonSerializer();

            try
            {
                var item = serializer.Deserialize<T>(jsonTextReader);

                return item;
            }
            catch (Exception ex) when (!ex.IsFatal())
            {
                logger.Error("Could not deserialize item {Type}", typeof(T));
                return default;
            }
        }
    }
}