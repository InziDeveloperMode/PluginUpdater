using CommandSystem;
using Exiled.API.Features;
using System;

namespace PluginUpdater.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class CheckUpdate: ICommand
    {
        public string Command => "checkupdate";

        public string[] Aliases { get; } = { "cu" };

        public string Description => "Update all plugins in the list of plugins to update";

        public bool SanitizeResponse => true;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
        
            EventsHandler.UpdatePlugins();
            response = "Done!";
            return true;
        }
    }
}
