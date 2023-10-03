using System.Threading.Tasks;

namespace Keyavi.Logs.LogAnalyticsWorkspace.Function.Core
{
    public interface IDataCollector<T>
    {
        Task CollectAsync(T item);
    }
}
