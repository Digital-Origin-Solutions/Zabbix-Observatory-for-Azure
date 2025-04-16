using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ZabbixSend.Models;
using Azure.Identity;
using Azure.Monitor.Query;
using Azure;
using System.Diagnostics;
using System.Net;
using Microsoft.Azure.Functions.Worker.Http;

namespace Observatory
{
    public class RecoveryServicesLog
    {
        private LogsQueryClient LogClient = new(new DefaultAzureCredential());

        [Function("GetBackupResult")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "backup_result/{*targetName}")] HttpRequestData req, String targetName)
        {
            try
            {
                // Get the Workspace ID from the App Settings in Azure.
                String? workspaceId = System.Environment.GetEnvironmentVariable("WorkspaceId", EnvironmentVariableTarget.Process);
                // Check if workspaceId is null. Throw a 500 if it is.
                if (workspaceId is not null)
                {
                    // Get the latest backup job for the backup item filtered by the targetName variable. Scoped to 1 day. Bound to the AddanAzureBackupJobsLogModel.
                    Response<IReadOnlyList<AddonAzureBackupJobsLogModel>> result = await LogClient.QueryWorkspaceAsync<AddonAzureBackupJobsLogModel>(
                        workspaceId,
                        String.Format("AddonAzureBackupJobs | extend TargetName = extract(\";([^;]+)$\", 1, BackupItemUniqueId) | summarize LatestBackupTime = max(TimeGenerated) by TargetName | join kind=inner (AddonAzureBackupJobs | extend TargetName = extract(\";([^;]+)$\", 1, BackupItemUniqueId)) on TargetName, $left.LatestBackupTime == $right.TimeGenerated | where TargetName == \"{0}\"", targetName.ToLower()),
                        new QueryTimeRange(TimeSpan.FromDays(1))
                    );
                    // Check if the result contained a row for the targetName.
                    if (result.Value.Count > 0)
                    {
                        // Return the single object.
                        return new OkObjectResult(result.Value[0]);
                    }
                    else
                    {
                        // Return a 404.
                        return new NotFoundObjectResult($"No entry for target ( {targetName} ) could be found.");
                    }
                }
                // If the workspace ID was null, throw a 500.
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
                // If any error occured while retrieving the Log results, throw a 500 with the stack trace as the response body.
                StackTrace st = new(ex, true);
                string Trace = st.ToString();
                return new ObjectResult(new ProblemDetails()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Internal Server Error",
                    Detail = $"{ex.Message} {Trace}"
                })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
