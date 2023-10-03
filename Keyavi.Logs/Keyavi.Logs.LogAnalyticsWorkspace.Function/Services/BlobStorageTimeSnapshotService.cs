using Azure.Storage.Blobs;
using Keyavi.Logs.LogAnalyticsWorkspace.Function.Core;
using Keyavi.Logs.LogAnalyticsWorkspace.Function.Options;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Keyavi.Logs.LogAnalyticsWorkspace.Function.Services
{
    internal class BlobStorageTimeSnapshotService : ITimeSnapshotService
    {
        private readonly AzureStorageAccountOptions _options;
        public BlobStorageTimeSnapshotService(IOptions<AzureStorageAccountOptions> options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options.Value;
        }

        private BlobContainerClient CreateClient() => new(_options.ConnectionString, _options.ContainerName);

        public async Task<TimeSnapshotSettings> GetLastSuccessullySyncedTimeSnapshotAsync()
        {
            var client = CreateClient();
            await client.CreateIfNotExistsAsync();
            var blobClient = client.GetBlobClient(_options.BlobName);
            if (await blobClient.ExistsAsync())
            {
                var content = await blobClient.DownloadContentAsync();
                var settings = content.Value.Content.ToObjectFromJson<TimeSnapshotSettings>();
                return settings;
            }
            var defaultTime = DateTime.UtcNow.AddDays(-2);

            return new TimeSnapshotSettings
            {
                LastSuccessullySyncedTimeSnapshot = defaultTime,
            };
        }

        public async Task SaveLastSuccessullySyncedTimeSnapshotAsync(TimeSnapshotSettings timeSnapshotSettings)
        {
            var client = CreateClient();
            await client.CreateIfNotExistsAsync();

            var blobClient = client.GetBlobClient(_options.BlobName);
            await blobClient.DeleteIfExistsAsync();
            await blobClient.UploadAsync(new BinaryData(JsonSerializer.Serialize(timeSnapshotSettings)));
        }
    }
}
