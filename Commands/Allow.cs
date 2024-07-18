using AutoUpdatePlugin;
using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginUpdater.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Allow : ICommand
    {
        public string Command => "allow";

        public string[] Aliases { get; } = { "allow" };

        public string Description => "Update all plugins in the list of plugins to update";

        public bool SanitizeResponse => true;


        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            try
            {
                string settingsContent = File.ReadAllText(Path.Combine(Main.Instance.Config.FolderPath, "settings.txt"));

                if (settingsContent.Contains("allow=false"))
                {
                    File.WriteAllText(Path.Combine(Main.Instance.Config.FolderPath, "settings.txt"), "allow=true", Encoding.Default);
                    response = "Updates authorized successfully, to activate the plugin update go to the config file.";
                    return true;
                }
                else if (settingsContent.Contains("allow=true"))
                {
                    response = "Updates are already authorized";
                    return true;
                }
                else
                {
                    response = "I can't understand if the updates are authorized or not, perhaps the settings.txt file was modified manually";
                    return false;
                }
            }catch (Exception ex)
            {
                response = $"Error: {ex}";
                return false;
            }
   
        }
    }
}
