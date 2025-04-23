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
    public class AdminRelationshipEntry
    {
        public required String Id { get; set; }
        public required String TenantDisplayName { get; set; }
        public required String RelationshipDisplayName { get; set; }
        public required DateTime ExpiryDateTime { get; set; }
    }

        public class AdminRelationships
    {
        private readonly GraphServiceClient _graphServiceClient = new(new DefaultAzureCredential(), ["https://graph.microsoft.com/.default"]);

        [Function("AdminRelationships")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetAdminRelationships")] HttpRequest req)
        {
            try
            {
                List<DelegatedAdminRelationship>? relationships = (await _graphServiceClient.TenantRelationships.DelegatedAdminRelationships.GetAsync())?.Value;

                if (relationships is not null)
                {
                    List<AdminRelationshipEntry> table = new();

                    foreach (DelegatedAdminRelationship relationship in relationships)
                    {
                        if (relationship.Id is not null && relationship.EndDateTime is not null && relationship.Status is not null)
                        {
                            if (relationship.Status == DelegatedAdminRelationshipStatus.Active)
                            {
                                table.Add(new()
                                {
                                    Id = relationship.Id,
                                    RelationshipDisplayName = relationship.DisplayName is not null ? relationship.DisplayName : "Unnamed",
                                    TenantDisplayName = relationship.Customer?.DisplayName is not null ? relationship.Customer.DisplayName : "Unnamed",
                                    ExpiryDateTime = ((DateTimeOffset)relationship.EndDateTime).DateTime
                                });
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
