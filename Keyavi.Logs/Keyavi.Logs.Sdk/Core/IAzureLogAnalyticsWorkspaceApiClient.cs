using Refit;
using System.Text.Json;

namespace Keyavi.Logs.Sdk.Core;

public interface IAzureLogAnalyticsWorkspaceApiClient
{
    [Get("/v1/workspaces/{params.workspaceId}/query?query={params.query}")]
    Task<JsonDocument> QueryAsync(GetAzureLogAnalyticsQueryParams @params);
}