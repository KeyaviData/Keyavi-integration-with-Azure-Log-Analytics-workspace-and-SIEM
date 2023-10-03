namespace Keyavi.Logs.LogAnalyticsWorkspace.Function.Options
{
    public class AzureLogWorkspaceOptions
    {
        public string BaseUrl { get; set;}
        public string QueryUrl { get; set; }
        public string WorkspaceId { get; set;}
        public string LogName { get; set;}
        public string SharedKey { get; set;}
    }
}
