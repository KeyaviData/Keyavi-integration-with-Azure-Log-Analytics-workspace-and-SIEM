namespace Keyavi.Logs.LogAnalyticsWorkspace.Function.Options
{
    public class KeyaviApiOptions
    {
        public string BaseUrl { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string[] Scopes { get; set; }

        public bool IsB2C { get; set; }

        public string? Authority { get; set; }
    }
}
