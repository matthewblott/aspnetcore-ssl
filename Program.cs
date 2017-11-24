using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace aspnetcore
{
  public class Program
  {
    public static void Main(string[] args)
    {
      BuildWebHost(args).Run();
    }

    public static IWebHost BuildWebHost(string[] args)
    {
      var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
      var currentDir = Directory.GetCurrentDirectory();

      var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("hosting.json", optional: true)
        .AddJsonFile($"hosting.{environment}.json", optional: true)
        .AddEnvironmentVariables()
        .Build();

      var urlSettings = config.GetSection("urls");
      var urls = urlSettings.Value.Split(';');
      var url = urls.First();
      var uri = new Uri(url);
      var port = uri.Port;

      var host = WebHost.CreateDefaultBuilder(args)
        .UseConfiguration(config)
        .UseContentRoot(currentDir)
        .UseStartup<Startup>()
        .UseKestrel(options =>
        {
          options.Listen(IPAddress.Loopback, port, listenOptions =>
          {
            var certificateSettings = config.GetSection("certificateSettings");
            var certificateFileName = certificateSettings.GetValue<string>("filename");
            var certificatePassword = certificateSettings.GetValue<string>("password");
            var cert = new X509Certificate2(certificateFileName, certificatePassword);

            listenOptions.UseHttps(cert);

          });

        })
        .Build();

      return host;

    }

  }

}