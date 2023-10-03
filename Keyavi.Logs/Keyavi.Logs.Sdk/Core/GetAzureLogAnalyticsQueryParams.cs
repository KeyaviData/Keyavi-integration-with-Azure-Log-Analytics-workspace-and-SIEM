using Refit;

namespace Keyavi.Logs.Sdk.Core;

public class GetAzureLogAnalyticsQueryParams
{
    [AliasAs("workspaceId")]
    public string WorkspaceId { get; set; }

    [AliasAs("query")]
    public string Query { get; set; }
}