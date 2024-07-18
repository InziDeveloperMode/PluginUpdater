using Exiled.API.Features;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.IO.Compression;
using Newtonsoft.Json.Linq;
using AutoUpdatePlugin;
using System.Reflection;
using static AutoUpdatePlugin.Main;
using Exiled.Loader.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using System.Text;
using MEC;
using System.Diagnostics.CodeAnalysis;
using UnityEngine.Assertions.Must;

namespace PluginUpdater
{
        internal class EventsHandler
        {

        static bool anyPluginToUpdate = false;

        internal void SendWarn()
        {
            string settingsContent = File.ReadAllText(Path.Combine(Main.Instance.Config.FolderPath, "settings.txt"));

            if (settingsContent.Contains("allow=false") && Main.warningsent == false)
            {
                Log.Warn($"Hey, thank you for installing PluginUpdater!\nTo enable automatic and manual plugin updating, type \"allow\" or go to {Main.Instance.Config.FolderPath}/settings.txt and where you see \"allow\" enter true instead of false\nDon't worry, even if you send \"allow\", the plugins must be enabled in the plugin settings to be installed automatically, they will NOT be installed unless you enable them.");
                Main.warningsent = true;

            }
            if (!anyPluginToUpdate && settingsContent.Contains("allow=true"))
            {
                Log.Warn("No plugins to update. Make sure at least one plugin is enabled for updates!");
            }
        }

        internal static void UpdatePlugins()
        {


            string permissionContent = File.ReadAllText(Path.Combine(Main.Instance.Config.FolderPath, "settings.txt"));

            if (permissionContent.Contains("allow=false")) return;

            var customPluginList = LoadCustomPluginList(Path.Combine(Main.Instance.Config.FolderPath, "Custom-Updater.yml"));
            var allPluginsToUpdate = Main.Instance.pluginsToUpdate.Concat(customPluginList);

;

            foreach (var pluginInfo in allPluginsToUpdate)
            {
                // Check if the plugin is installed before attempting the update
                if (IsPluginInstalled(pluginInfo.Name))
                {
                    try
                    {
                        // Check if the update is available
                        if (IsUpdateAvailable(pluginInfo))
                        {
                            if (permissionContent.Contains("allow=true"))
                            {
                                if (Main.BlacklistedPluginNames.Contains(pluginInfo.Name, StringComparer.OrdinalIgnoreCase))
                                {
                                    Log.Warn($"{pluginInfo.Name} cannot be updated because it is blacklisted!");
                                }
                                else
                                {
                                    if (ShouldUpdatePlugin(pluginInfo))
                                    {
                                        Log.Warn($"Checking the plugin {pluginInfo.Name}....");
                                        UpdatePlugin(pluginInfo);
                                        anyPluginToUpdate = true;

                                        if (customPluginList.Any(p => p.Name.Equals(pluginInfo.Name, StringComparison.OrdinalIgnoreCase)))
                                        {
                                            Log.Warn($"Plugin {pluginInfo.Name} from custom list updated!");
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Log.Debug($"{pluginInfo.Name} is already updated to the latest version.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Error updating plugin: {ex.Message}");

                        if (ex is WebException webEx && webEx.Response is HttpWebResponse response)
                        {
                            if (response.StatusCode == HttpStatusCode.NotFound)
                            {
                                Log.Error("Error 404: Unable to find update. Make sure the URL is correct.");
                            }
                            else
                            {
                                Log.Error($"HTTP Error: {response.StatusCode}");
                            }
                        }
                        else if (ex is WebException WebEx && WebEx.Status == WebExceptionStatus.NameResolutionFailure)
                        {
                            Log.Error("Name resolution error: Make sure your server has a working network connection.");
                        }
                        else
                        {
                            Log.Error($"Unknown error during HTTP request: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Log.Debug($"{pluginInfo.Name} isn't installed on the server. The update will be ignored.");
                }
            }
        }


        private static bool ShouldUpdatePlugin(PluginInfo pluginInfo)
        {
            var customPluginList = LoadCustomPluginList(Path.Combine(Main.Instance.Config.FolderPath, "Custom-Updater.yml"));

            switch (pluginInfo.Name)
            {
                case "DotaHeroes" when Main.Instance.Config.DotaHeroes:
                case "DiscordIntegration" when Main.Instance.Config.DiscordIntegration:
                case "CustomItems" when Main.Instance.Config.CustomItems:
                case "CustomRoles" when Main.Instance.Config.CustomRoles:
                case "SCPCosmetics" when Main.Instance.Config.SCPCosmetics:
                case "UncomplicatedCustomRoles" when Main.Instance.Config.UncomplicatedCustomRoles:
                case "MapEditorReborn" when Main.Instance.Config.MapEditorReborn:
                case "BetterRestartingSystem" when Main.Instance.Config.BetterRestartingSystem:
                case "ScriptedEvents" when Main.Instance.Config.ScriptedEvents:
                case "BetterSinkholes" when Main.Instance.Config.BetterSinkholes:
                case "AutoBroadcast" when Main.Instance.Config.AutoBroadcast:
                case "ShootingInteractions" when Main.Instance.Config.ShootingInteractions:
                case "RoundReports" when Main.Instance.Config.RoundReports:
                case "FacilityManagement" when Main.Instance.Config.FacilityManagement:
                    return true;
                default:
                    if (customPluginList.Any(p => p.Name.Equals(pluginInfo.Name, StringComparison.OrdinalIgnoreCase)))
                        return true;
                    else
                        return false;
            }
        }


        internal static bool IsPluginInstalled(string pluginName)
        {
            // Check if the plugin DLL file exists in the plugins directory
            string pluginPath = Path.Combine(Paths.Plugins, $"{pluginName}.dll");
            return File.Exists(pluginPath);
        }

        internal static bool IsUpdateAvailable(PluginInfo pluginInfo)
        {
            // It will be updated in the next version

            return true;
        }





        internal static void UpdatePlugin(PluginInfo pluginInfo)
        {

            // Get the URL of the DLL file from the GitHub repository
            string dllUrl = $"{pluginInfo.GitHubRepoUrl}/releases/latest/download/{pluginInfo.Name}.dll";

            try
            {
                DownloadFile(dllUrl, Path.Combine(Paths.Plugins, $"{pluginInfo.Name}.dll"));
                Log.Info($"{pluginInfo.Name} validated and updated to the latest version!");
            }
            catch (WebException ex)
            {
                Log.Error($"Error downloading file: {ex.Message}");

            }
        
         }



        internal static void DownloadFile(string fileUrl, string savePath)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(fileUrl, savePath);
            }
        }

        internal static void CreatePLuginUpdaterFiles()
        {
            try
            {

                if (!Directory.Exists(Main.Instance.Config.FolderPath))
                {
                    Directory.CreateDirectory(Main.Instance.Config.FolderPath);
                }

                if (!File.Exists(Path.Combine(Main.Instance.Config.FolderPath, "Custom-Updater.yml")))
                {
            
                    var customPluginList = new List<PluginInfo>
                {
                    new PluginInfo { Name = "CustomPlugin1", GitHubRepoUrl = "https://github.com/user/repo1" },
                    new PluginInfo { Name = "CustomPlugin2", GitHubRepoUrl = "https://github.com/user/repo2" },
                    
                };
               
                    var serializer = new Serializer();
                    var yamlContent = serializer.Serialize(customPluginList);

                    File.WriteAllText(Path.Combine(Main.Instance.Config.FolderPath, "Custom-Updater.yml"), yamlContent, Encoding.UTF8);

                    Log.Warn($"I am creating Custom-Updater.yml file in the PluginUpdater folder....");
                    
                }
                else
                {
                    Log.Debug($"Custom-Updater.yml file already exists in the config folder.");
                }

                if(!File.Exists(Path.Combine(Main.Instance.Config.FolderPath, "settings.txt")))
                {
                 
                    File.WriteAllText(Path.Combine(Main.Instance.Config.FolderPath, "settings.txt"), "allow=false", Encoding.Default);
                    Log.Warn($"I am creating settings.txt file in the PluginUpdater folder....");
                }
            }
            catch (Exception ex)
            {
                
                Log.Error($"Error creating YAML file: {ex.Message}");
            }
        }


        internal static PluginInfo[] LoadCustomPluginList(string filePath)
        {
            try
            {
               
                if (!File.Exists(filePath))
                {
                    Log.Warn($"File {filePath} not found. Creating an empty list.");
                    return Array.Empty<PluginInfo>();
                }


                using (var reader = new StreamReader(filePath))
                {
                    var deserializer = new Deserializer();
                    var customPluginList = deserializer.Deserialize<List<PluginInfo>>(reader);
                    return customPluginList?.ToArray() ?? Array.Empty<PluginInfo>();
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error loading YAML file: {ex.Message}");
                return Array.Empty<PluginInfo>();
            }
        }






    }
}
