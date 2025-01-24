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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using static Grpc.Core.Metadata;
using System.Net;
using Microsoft.Azure.Functions.Worker.Http;

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

        [Function("GetBackupResult")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "backup_result/{*targetName}")] HttpRequestData req, String targetName)
        {
            try
            {
                String? workspaceId = System.Environment.GetEnvironmentVariable("WorkspaceId", EnvironmentVariableTarget.Process);
                if (workspaceId is not null)
                {
                    Response<IReadOnlyList<AddonAzureBackupJobsLogModel>> result = await LogClient.QueryWorkspaceAsync<AddonAzureBackupJobsLogModel>(
                        workspaceId,
                        String.Format("AddonAzureBackupJobs | extend TargetName = extract(\";([^;]+)$\", 1, BackupItemUniqueId) | summarize LatestBackupTime = max(TimeGenerated) by TargetName | join kind=inner (AddonAzureBackupJobs | extend TargetName = extract(\";([^;]+)$\", 1, BackupItemUniqueId)) on TargetName, $left.LatestBackupTime == $right.TimeGenerated | where TargetName == \"{0}\"", targetName),
                        new QueryTimeRange(TimeSpan.FromDays(1))
                    );
                    return new OkObjectResult(result.Value[0]);
                }
                return new ObjectResult(new ProblemDetails ()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Internal Server Error",
                    Detail = "The WorkspaceId environment variable could not be resolved. Please validate it's value."
                })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
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
