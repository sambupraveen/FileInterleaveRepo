using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            // configure serilog
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Build())
                                                  .Enrich.FromLogContext()
                                                  .WriteTo.Console()
                                                  .Enrich.WithMachineName()
                                                  .CreateLogger();

            var host = Host.CreateDefaultBuilder().ConfigureServices((context,services) =>
            {
                services.AddTransient<IFileService, FileService>();
            }).UseSerilog().Build();
            var svc = ActivatorUtilities.CreateInstance<FileService>(host.Services);
            _ = svc.Read();
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            var Configuration = new ConfigurationBuilder()
                                                   .SetBasePath(Directory.GetCurrentDirectory())
                                                   .AddEnvironmentVariables()
                                                   .Build();
        }
    }
}
