using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyChy.Frame.Core.Web3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("config/hosting.json", optional: true)
                    .Build();

                    webBuilder.UseConfiguration(config);
                    webBuilder.UseStartup<Startup>();
                });




 

/// <summary>
/// Check if this process is running on Windows in an in process instance in IIS
/// </summary>
/// <returns>True if Windows and in an in process instance on IIS, false otherwise</returns>
public static bool IsRunningInProcessIIS()
{
    if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        return false;
    }

    string processName = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().ProcessName);
    return (processName.Contains("w3wp", StringComparison.OrdinalIgnoreCase) ||
        processName.Contains("iisexpress", StringComparison.OrdinalIgnoreCase));

}
    }
}
