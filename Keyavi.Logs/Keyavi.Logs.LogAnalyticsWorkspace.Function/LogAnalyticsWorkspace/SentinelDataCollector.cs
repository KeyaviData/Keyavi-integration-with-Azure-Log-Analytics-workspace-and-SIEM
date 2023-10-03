using Keyavi.Logs.LogAnalyticsWorkspace.Function.Core;
using Keyavi.Logs.LogAnalyticsWorkspace.Function.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;

namespace Keyavi.Logs.LogAnalyticsWorkspace.Function.LogAnalyticsWorkspace
{
    public  class SentinelDataCollector<T> : IDataCollector<T>
    {
        private readonly ILogger _logger;
        private readonly IOptions<AzureLogWorkspaceOptions> _options;
        private HttpClient client;

        public SentinelDataCollector(ILogger logger, IOptions<AzureLogWorkspaceOptions> options, IHttpClientFactory httpClientFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));

            client = httpClientFactory.CreateClient("Sentinel");
        }

        private Uri BaseUri => new Uri(string.Format(_options.Value.BaseUrl, _options.Value.WorkspaceId));

        public async Task CollectAsync(T data)
        {
            var rawString = JsonSerializer.Serialize(data, new JsonSerializerOptions { 
                WriteIndented = false,
            });
            _logger.LogInformation("Collecting data, this stream will be sent over HTTP to the Azure Log Workspace");

            var datestring = DateTime.UtcNow.ToString("r");

            string stringToHash = "POST\n" + rawString.Length + "\napplication/json\n" + "x-ms-date:" + datestring + "\n/api/logs";
            string hashedString = BuildSignature(stringToHash, _options.Value.SharedKey);
            string signature = "SharedKey " + _options.Value.WorkspaceId + ":" + hashedString;

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Log-Type", _options.Value.LogName);
            client.DefaultRequestHeaders.Add("Authorization", signature);
            client.DefaultRequestHeaders.Add("x-ms-date", datestring);
            client.DefaultRequestHeaders.Add("time-generated-field", "LogTimestamp"); // This is required because Azure internally uses this to trace events
            // More info about what each header does can be found here https://learn.microsoft.com/en-us/azure/azure-monitor/logs/data-collector-api?tabs=powershell#request-headers

            using var httpContent = new StringContent(rawString);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            using var response = await client.PostAsync(BaseUri, httpContent);
            response.EnsureSuccessStatusCode();
        }

        public static string BuildSignature(string message, string secret)
        {
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = Convert.FromBase64String(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using var hmacsha256 = new HMACSHA256(keyByte);
            byte[] hash = hmacsha256.ComputeHash(messageBytes);
            return Convert.ToBase64String(hash);
        }
    }
}
