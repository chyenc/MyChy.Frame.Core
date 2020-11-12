using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.Web5
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
                    if (!IsRunningInProcessIIS())
                    {
                        //////设置超时时间 20分钟
                        webBuilder.UseKestrel(option =>
                        {
                            option.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(20);
                            option.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(20);

                            //413 Request Entity Too Large（请求实体太大）
                            option.Limits.MaxRequestBodySize = null;
                        });

                    }

                    webBuilder.UseStartup<Startup>();
                });


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
