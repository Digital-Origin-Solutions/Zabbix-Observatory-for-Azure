using Azure.Identity;
using Azure.Monitor.Query.Models;
using Azure.Monitor.Query;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Diagnostics;
using ZabbixSend.Models;
using ZabbixSend;
using Microsoft.Extensions.Configuration;

// Instantiate the HTTP server settings.
// Will use any methods from any class that are anotated with [Function()].
var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .Build();

// Start the HTTP server.
host.Run();
