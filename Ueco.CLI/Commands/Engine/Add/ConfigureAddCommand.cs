using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ueco.Services;

namespace Ueco.Commands.Engine.Add;

public static class ConfigureAddCommand
{
    public static void AddAddCommand(this CliCommand command)
    {
        var addCommand = new CliCommand("add", "Add unreal engine install")
        {
            new CliArgument<string>("name")
            {
                Description = "Name of the engine",
            },
            new CliArgument<FileInfo>("path")
            {
                Description = "Path to the engine",
            },
            new CliOption<bool>("--isDefault")
            {
                Description = "Set this engine as default",
                Required = false
            }
        };
        
        addCommand.Action = CommandHandler.Create<string, bool, FileInfo, IHost>((name, isDefault, path, host) =>
        {
            var serviceProvider = host.Services;
            
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("Engine.ListCommand");
            
            var unrealEngineAssociationRepository = serviceProvider.GetRequiredService<IUnrealEngineAssociationRepository>();
           
            AddCommand.Execute(name, path, isDefault, unrealEngineAssociationRepository, logger);
        });
        
        command.Add(addCommand);
    }
}