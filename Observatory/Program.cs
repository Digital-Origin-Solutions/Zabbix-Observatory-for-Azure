using Microsoft.Extensions.Hosting;

// Instantiate the HTTP server settings.
// Will use any methods from any class that are anotated with [Function()].
var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .Build();

// Start the HTTP server.
host.Run();
