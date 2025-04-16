using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Diagnostics;
using System.Net;

namespace Observatory
{
    public class ApplicationSecretEntry
    {
        public required String ObjectId { get; set; }
        public required String SecretDisplayName { get; set; }
        public required String ApplicationRegistrationDisplayName { get; set; }
        public required Guid SecretId { get; set; }
        public required DateTime ExpiryDateTime { get; set; }
    }
    public class OrganisationalApplications
    {
        private readonly GraphServiceClient _graphServiceClient = new(new DefaultAzureCredential(), ["https://graph.microsoft.com/.default"]);

        [Function("GetOrganisationalApplicationSecrets")]
        public async Task<IActionResult> GetOrganisationalApplicationSecrets([HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetOrganisationalApplicationSecrets")] HttpRequest req)
        {
            try
            {
                List<Application>? apps = (await _graphServiceClient.Applications.GetAsync())?.Value;

                if (apps is not null)
                {
                    List<ApplicationSecretEntry> table = new();

                    foreach (Application app in apps)
                    {
                        if (app.PasswordCredentials is not null)
                        {
                            foreach (PasswordCredential secret in app.PasswordCredentials)
                            {
                                if (app.Id is not null && secret.KeyId is not null && secret.EndDateTime is not null)
                                {
                                    table.Add(new()
                                    {
                                        ObjectId = app.Id,
                                        SecretDisplayName = secret.DisplayName is not null ? secret.DisplayName : "Unnamed",
                                        ApplicationRegistrationDisplayName = app.DisplayName is not null ? app.DisplayName : "Unnamed",
                                        SecretId = (Guid)secret.KeyId,
                                        ExpiryDateTime = ((DateTimeOffset)secret.EndDateTime).DateTime
                                    });
                                }
                            }
                        }
                    }
                    return new OkObjectResult(table);
                } 
                else
                {
                    // Return a 404.
                    return new NotFoundObjectResult($"App registrations could not be retrieved.");
                }
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
