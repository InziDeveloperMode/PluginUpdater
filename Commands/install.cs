using AutoUpdatePlugin;
using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutoUpdatePlugin.Main;

namespace PluginUpdater.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Install : ICommand
    {
        public string Command => "install";

        public string[] Aliases { get; } = { "intl" };

        public string Description => "install a plugin with a command";

        public bool SanitizeResponse => true;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string settingsContent = File.ReadAllText(Path.Combine(Main.Instance.Config.FolderPath, "settings.txt"));

            if (settingsContent.Contains("allow=false"))
            {
                response = "Unauthorized installation, please send \"allow\" command before installing";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "You must specify a plugin name to install or use 'list' to see available plugins. Example: install MER";
                return false;
            }

            string firstArgument = arguments.At(0).ToLower();

            if (firstArgument == "list")
            {
                response = ListAvailablePlugins();
                return true;
            }

            string pluginName = arguments.At(0);
            var pluginInfo = FindPluginByName(pluginName);

            if (pluginInfo == null)
            {
                response = $"Plugin {pluginName} not found in the list of available plugins.";
                return false;
            }

            try
            {
                if (EventsHandler.IsPluginInstalled(pluginName))
                {
                    response = $"Plugin {pluginName} is already installed.";
                    return false;
                }

                EventsHandler.UpdatePlugin(pluginInfo);
                response = $"Plugin {pluginName} successfully installed.";
                return true;
            }
            catch (Exception ex)
            {
                response = $"Error during plugin installation: {ex.Message}";
                return false;
            }
        }

        private PluginInfo FindPluginByName(string name)
        {
            // Searches for the plugin in the list of available plugins
            var pluginList = Main.Instance.pluginsToUpdate;
            return pluginList.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        private string ListAvailablePlugins()
        {
            // Lists all available plugins for installation from pluginsToUpdate
            var pluginList = Main.Instance.pluginsToUpdate;

            // Log each available plugin
            Log.Info("Available plugins for installation:");
            foreach (var plugin in pluginList)
            {
                Log.Info($"- {plugin.Name}");
            }

            return "Use install [pluginName] to install a plugin.";
        }

    }
}
