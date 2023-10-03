using Keyavi.Logs.LogAnalyticsWorkspace.Function.Core;
using Keyavi.Logs.Sdk.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Keyavi.Logs.LogAnalyticsWorkspace.Function.Services
{
    public class PullFromKeyaviApiLogSyncService : ILogSyncService
    {
        private readonly int defaultMaxPageSize = 1000;
        private readonly IKeyaviAuditApiClient _auditApiClient;
        private readonly int batchSize = 100;

        private readonly IDataCollector<List<FullPayloadLog>> _dataCollector;
        private readonly ILogger _logger;
        private readonly ITimeSnapshotService _timeSnapshotService;

        public PullFromKeyaviApiLogSyncService(IKeyaviAuditApiClient auditApiClient, IDataCollector<List<FullPayloadLog>> dataCollector, ILogger logger, ITimeSnapshotService timeSnapshotService)
        {
            _auditApiClient = auditApiClient ?? throw new ArgumentNullException(nameof(auditApiClient));
            _dataCollector = dataCollector ?? throw new ArgumentNullException(nameof(dataCollector));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _timeSnapshotService = timeSnapshotService ?? throw new ArgumentNullException(nameof(timeSnapshotService));
        }



        public async Task SyncAsync() => await SyncNextBatch(1, defaultMaxPageSize, await _timeSnapshotService.GetLastSuccessullySyncedTimeSnapshotAsync());

        private async Task SyncNextBatch(int page, int pageSize, TimeSnapshotSettings timeSnapshotSettings)
        {
            var logsResponse = await _auditApiClient.GetFullPayloadLogsAsync(new GetFullPayloadLogsQueryParams
            {
                Page = page,
                PageSize = pageSize,
                StartTime = timeSnapshotSettings.LastSuccessullySyncedTimeSnapshot.ToString(),
                Sort = "+LogTimestamp"
            });

            _logger.LogDebug("Current page: {0}", JsonSerializer.Serialize(logsResponse));

            if (logsResponse == null || logsResponse.Data == null)
            {
                _logger.LogInformation($"No data found.");
                return;
            }

            var items = logsResponse.Data.ToList();

            if (timeSnapshotSettings.LastSuccessullySyncedLogId != null && timeSnapshotSettings.LastSuccessullySyncedLogId > 0)
            {
                items = items.Where(i => i.LogId > timeSnapshotSettings.LastSuccessullySyncedLogId).ToList();
            }

            if (!items.Any())
            {
                _logger.LogInformation($"No more pages found. Last page was {page}.");
                return;
            }
            _logger.LogInformation($"Sending logs to Sentinel for page {page} started.");

            var iteration = 0;
            while (batchSize * iteration <= items.Count)
            {
                var batch = items.Skip(batchSize * iteration).Take(batchSize).ToList();
                await _dataCollector.CollectAsync(batch);
                iteration++;

                var last = batch.Last();
                await _timeSnapshotService.SaveLastSuccessullySyncedTimeSnapshotAsync(new TimeSnapshotSettings
                {
                    LastSuccessullySyncedTimeSnapshot = last.LogTimestamp,
                    LastSuccessullySyncedLogId = last.LogId,
                });
            }

            _logger.LogInformation($"Sending logs to Sentinel for page {page} finished.");

            if (logsResponse.Pagination.IsLastPage())
            {
                var last = items.Last();
                await _timeSnapshotService.SaveLastSuccessullySyncedTimeSnapshotAsync(new TimeSnapshotSettings
                {
                    LastSuccessullySyncedTimeSnapshot = last.LogTimestamp,
                    LastSuccessullySyncedLogId = last.LogId,
                });
                _logger.LogInformation($"Last page found. Sync completed.");
                return;
            }
            var nextPage = page + 1;
            _logger.LogInformation($"Syncing next page ${nextPage}.");
            await SyncNextBatch(nextPage, pageSize, timeSnapshotSettings);

        }
    }
}
