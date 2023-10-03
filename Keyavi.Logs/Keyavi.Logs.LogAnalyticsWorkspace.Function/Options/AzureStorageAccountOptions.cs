namespace Keyavi.Logs.LogAnalyticsWorkspace.Function.Options
{
    public class AzureStorageAccountOptions
    {
        public string ContainerName { get; set; }

        public string ConnectionString { get; set; }

        public string BlobName { get; set; }
    }
}
