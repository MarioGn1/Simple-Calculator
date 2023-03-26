using Calculator.Endpoints.Calculate;
using Calculator.IntegrationTests.Exceptions;
using System.Net.Http.Json;
using System.Text.Json;

namespace Calculator.IntegrationTests.Utils
{
    internal static class HttpClientHelper
    {
        private static readonly string CalculateRoute = CalculateEndpoint.GetRoute();

        public static async Task<double?> Calculate(this HttpClient client, CalculateRequest? request = default)
            => await client.PostContentAsync<double>(CalculateRoute, JsonContent.Create(request));

        private static async Task<T?> PostContentAsync<T>(this HttpClient httpClient, string url, HttpContent postData)
        {
            var response = await httpClient.PostAsync(url, postData);

            await response.EnsureStatusCode();

            var content = await response.ExtractContent<T>();

            return content;
        }

        public static async Task EnsureStatusCode(this HttpResponseMessage response, HttpStatusCode? statusCode = null)
        {
            if (statusCode.HasValue && response.StatusCode == statusCode) return;

            if (new[] { HttpStatusCode.OK, HttpStatusCode.NoContent, HttpStatusCode.Created }.Contains(response.StatusCode))
                return;

            var content = await response.Content.ReadAsStringAsync();

            throw new HttpResponseMessageStatusCodeInvalidException(statusCode, response.StatusCode, content);
        }

        public static async Task<T?> ExtractContent<T>(this HttpResponseMessage httpResponse)
        {
            await httpResponse.EnsureStatusCode();

            if (httpResponse.StatusCode == HttpStatusCode.NoContent)
                return default;

            var contentString = await httpResponse.Content.ReadAsStringAsync();

            var content = contentString.Deserialize<T>();

            return content;
        }

        private static TResult? Deserialize<TResult>(this string jsonContent) =>
            JsonSerializer.Deserialize<TResult>(jsonContent, AssignDefaultOptions(new JsonSerializerOptions()));

        private static JsonSerializerOptions AssignDefaultOptions(JsonSerializerOptions options)
        {
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            return options;
        }
    }
}
