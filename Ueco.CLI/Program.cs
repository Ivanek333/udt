﻿using System.CommandLine;
using System.CommandLine.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Ueco.Commands;
using Ueco.Common;

namespace Ueco;

public static class  Program
{
    private static Task Main(string[] args)
    {
        var cliConfiguration = BuildCommandLine();
        cliConfiguration.UseHost(_ => Host.CreateDefaultBuilder(),
                host =>
                {
                    host.ConfigureLogging(builder =>
                    {
                        builder.ConfigureCustomFormatter();

                        var parseResult = cliConfiguration.Parse(args);
                        var debugLog = parseResult.GetValue<bool>("--debug-log");
                        if (debugLog)
                        {
                            builder.AddFilter("Microsoft.Hosting", LogLevel.Information);
                        }
                    });
                });

        return cliConfiguration.InvokeAsync(args);
    }
    
    private static CliConfiguration BuildCommandLine()
    {
        return new CliConfiguration(ConfigureRootCommand.AddRootCommand());
    }

    private static void ConfigureCustomFormatter(this ILoggingBuilder builder)
    {

        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        const string color = "Logging:Console:FormatterOptions:Colors:";

        var traceColor = Enum.TryParse(config[color + "Trace"], out ConsoleColor parsedTraceColor);
        var informationColor = Enum.TryParse(config[color + "Information"], out ConsoleColor parsedInformationColor);
        var warningColor = Enum.TryParse(config[color + "Warning"], out ConsoleColor parsedWarningColor);
        var errorColor = Enum.TryParse(config[color + "Error"], out ConsoleColor parsedErrorColor);
        var debugColor = Enum.TryParse(config[color + "Debug"], out ConsoleColor parsedDebugColor);

        Enum.TryParse(config["Logging:Console:FormatterOptions:ColorBehavior"], out LoggerColorBehavior parsedColorBehavior);

        builder.AddCustomFormatter(options =>
        {
            options.ColorBehavior = parsedColorBehavior;
            options.TraceColor = traceColor ? parsedTraceColor : Console.ForegroundColor;
            options.WarningColor = warningColor ? parsedWarningColor : Console.ForegroundColor;
            options.ErrorColor = errorColor ? parsedErrorColor : Console.ForegroundColor;
            options.DebugColor = debugColor ? parsedDebugColor : Console.ForegroundColor;
            options.InformationColor = informationColor ? parsedInformationColor : Console.ForegroundColor;
            options.UseUtcTimestamp = config["Logging:Console:FormatterOptions:UseUtcTimestamp"] == "true";
        });
    }
}