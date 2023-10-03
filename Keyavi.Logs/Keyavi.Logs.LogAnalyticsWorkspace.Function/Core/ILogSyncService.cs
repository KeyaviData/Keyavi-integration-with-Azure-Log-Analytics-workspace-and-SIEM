using System.Threading.Tasks;

namespace Keyavi.Logs.LogAnalyticsWorkspace.Function.Core;

public interface ILogSyncService
{
    public Task SyncAsync();
}
