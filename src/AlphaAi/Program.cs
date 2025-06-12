using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using AlphaAi.Configuration.Options;
using AlphaAi.Configuration.TypedConfigurations;
using AlphaAi.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace AlphaAi;

[SuppressMessage("Maintainability", "CA1515:Consider making public types internal")]
[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
[SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable")]
public class Program
{
    [SuppressMessage("ReSharper", "ConvertToUsingDeclaration")]
    [SuppressMessage("Style", "IDE0063:Use simple \'using\' statement")]
    [SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates")]
    public static int Main(string[] args)
    {
        var returnCode = 0;
        Console.OutputEncoding = Encoding.UTF8;

        var builder = Host.CreateApplicationBuilder(args);
        builder.Logging.ClearProviders();
        builder.Logging.AddConfiguration(builder.Configuration.GetRequiredSection("Logging"));
        builder.Logging.AddSimpleConsole(options =>
        {
            options.ColorBehavior = LoggerColorBehavior.Enabled;
            options.UseUtcTimestamp = true;
            options.IncludeScopes = true;
        });
        var exceptionLogged = false;
        try
        {
            var config = builder.Configuration
                .GetTypedConfigurationFromOptions<ApplicationOptions, ApplicationConfiguration>(static x =>
                    ApplicationConfiguration.Convert(x));
            builder.Services.AddSingleton(config);
            builder.Services.AddHostedService<Worker>();
            using (var host = builder.Build())
            {
                try
                {
                    host.Run();
                }
                catch (Exception ex)
                {
                    var programLogger = host.Services.GetRequiredService<ILogger<Program>>();
                    programLogger.LogCritical(ex, "Program terminated unexpectedly");
                    exceptionLogged = true;
                    throw;
                }
            }
        }
        catch (Exception ex)
        {
            if (!exceptionLogged)
            {
                Console.WriteLine(ex);
            }

            returnCode = -1;
        }

        return returnCode;
    }
}
