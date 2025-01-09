using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ZabbixSend.Models;
using Azure.Identity;
using Azure.Monitor.Query;
using Azure.Monitor.Query.Models;
using Azure;
using System.Diagnostics;

namespace ZabbixSend
{
    public class ZabbixSend
    {
        private readonly ILogger<ZabbixSend> _logger;
        private LogsQueryClient LogClient = new(new DefaultAzureCredential());

        public ZabbixSend(ILogger<ZabbixSend> logger)
        {
            _logger = logger;
        }

        [Function("ZabbixSend")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            try
            {
                Response<LogsQueryResult> result = await LogClient.QueryWorkspaceAsync(
                    "620adbd1-5523-4694-b864-b3a952ee47f1",
                    "AddonAzureBackupJobs | summarize arg_max(TimeGenerated, *) by JobUniqueId | order by TimeGenerated | limit 1 ",
                    new QueryTimeRange(TimeSpan.FromDays(1))
                );
                var tableRows = result.Value.Table.Rows[0];
                //ZabbixRequest request = new("zabbix.digital-origin.co.uk", 10051);
                AddonAzureBackupJobsLogEntry entry = new()
                {
                    JobUniqueId = (string)tableRows[0],
                    TimeGenerated = (DateTimeOffset)tableRows[1],
                    TenantId = (string)tableRows[2],
                    SourceSystem = (string)tableRows[3],
                    ResourceId = (string)tableRows[4],
                    OperationName = (string)tableRows[5],
                    Category = (string)tableRows[6],
                    AdHocOrScheduledJob = (string)tableRows[7],
                    BackupItemUniqueId = (string)tableRows[8],
                    TargetName = ((string)tableRows[8]).Split(";")[4],
                    Region = ((string)tableRows[8]).Split(";")[0],
                    BackupManagementServerUniqueId = (string)tableRows[9],
                    BackupManagementType = (string)tableRows[10],
                    DataTransferredInMB = (double)tableRows[11],
                    JobDurationInSecs = (double)tableRows[12],
                    JobFailureCode = (string)tableRows[13],
                    JobOperation = (string)tableRows[14],
                    JobOperationSubType = (string)tableRows[15],
                    JobStartDateTime = (DateTimeOffset)tableRows[16],
                    JobStatus = (string)tableRows[17],
                    ProtectedContainerUniqueId = (string)tableRows[18],
                    RecoveryJobDestination = (string)tableRows[19],
                    RecoveryJobRPDateTime = (string)tableRows[20],
                    RecoveryJobRPLocation = (string)tableRows[21],
                    RecoveryLocationType = (string)tableRows[22],
                    SchemaVersion = (string)tableRows[23],
                    State = (string)tableRows[24],
                    VaultUniqueId = (string)tableRows[25],
                    DatasourceSetFriendlyName = (string)tableRows[26],
                    DatasourceSetResourceId = (string)tableRows[27],
                    DatasourceSetType = (string)tableRows[28],
                    DatasourceResourceId = (string)tableRows[29],
                    DatasourceType = (string)tableRows[30],
                    DatasourceFriendlyName = (string)tableRows[31],
                    SubscriptionId = (string)tableRows[32],
                    ResourceGroupName = (string)tableRows[33],
                    VaultName = (string)tableRows[34],
                    VaultTags = (string)tableRows[35],
                    VaultType = (string)tableRows[36],
                    StorageReplicationType = (string)tableRows[37],
                    ArchiveTierStorageReplicationType = (string)tableRows[38],
                    AzureDataCenter = (string)tableRows[39],
                    BackupItemId = (string)tableRows[40],
                    BackupItemFriendlyName = (string)tableRows[41],
                    ExtendedProperties = (string)tableRows[42],
                    Type = (string)tableRows[43],
                    _ResourceId = (string)tableRows[44]
                };

                //await request.Send("SendHost", "Azure.Backup.Status", JsonConvert.SerializeObject(entry));
                return new OkObjectResult(entry);
            }
            catch (Exception ex)
            {
                StackTrace st = new(ex, true);
                string Trace = st.ToString();
                _logger.LogError("{Message} {Trace}", ex.Message, Trace);
                return new BadRequestObjectResult($"{ex.Message} {Trace}");
            }
        }
    }
}
