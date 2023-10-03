using System.Text.Json.Serialization;

namespace Keyavi.Logs.Sdk.Core;


public class PagedResult<T>
{
    [JsonPropertyName("pagination")]
    public Pagination Pagination { get; set; } = new Pagination();
    public List<T> Data { get; set; } = new List<T>();
}

