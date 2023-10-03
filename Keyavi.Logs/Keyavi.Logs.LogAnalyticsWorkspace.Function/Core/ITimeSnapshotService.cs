using System;
using System.Threading.Tasks;

namespace Keyavi.Logs.LogAnalyticsWorkspace.Function.Core;

public interface ITimeSnapshotService
{
    /// <summary>
    /// This method must return the last time the sync service succesfully sent the last batch of logs from the previous run, 
    /// That's the reason the suffix "snapshot" has been added to the name, even though the actual lastSuccessfullySyncedTime is updated multiple times
    /// While the process is running, the whole process must used the same lastSuccessfullySyncedTimeSnapshot until it finishes running
    /// So it can iterate over all the pages left since lastSuccessfullySyncedTimeSnapshot without alterations
    /// </summary>
    /// <returns>Return the last time the sync service succesfully sent the last batch of logs from the previous run</returns>
    public Task<TimeSnapshotSettings> GetLastSuccessullySyncedTimeSnapshotAsync();

    /// <summary>
    /// This process is a stores a "check point" of what was the last timestamp of the record that was successfully synced
    /// Make sure to always set the value coming from the timestmap of the last record (from the batch)
    /// </summary>
    /// <param name="lastSuccessfullySyncedTime"></param>
    /// <returns></returns>
    public Task SaveLastSuccessullySyncedTimeSnapshotAsync(TimeSnapshotSettings timeSnapshotSettings);
}