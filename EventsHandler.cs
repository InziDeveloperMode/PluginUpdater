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

namespace PluginUpdater
{
        internal class EventsHandler
        {

        internal static void UpdatePlugins()
        {

            var customPluginList = LoadCustomPluginList(Path.Combine(Exiled.API.Features.Paths.Configs, "Custom-Updater.yml"));

            var allPluginsToUpdate = Main.Instance.pluginsToUpdate.Concat(customPluginList);


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
                            if (pluginInfo.Name == "PluginUpdater")
                            {
                                Timing.CallDelayed(6f, () => {
                                    Log.Warn($"Checking the plugin {pluginInfo.Name}....");
                                    UpdatePlugin(pluginInfo);
                                    }
                                );

                            }
                            else
                            {
                                Log.Warn($"Checking the plugin {pluginInfo.Name}....");
                                UpdatePlugin(pluginInfo);
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

                        // If the exception is a WebException, handle it specifically
                        if (ex is WebException webEx)
                        {
                            if (webEx.Response is HttpWebResponse response)
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
                            else if (webEx.Status == WebExceptionStatus.NameResolutionFailure)
                            {
                                Log.Error("Name resolution error: Make sure your server has a working network connection.");
                            }
                            else
                            {
                                Log.Error($"Unknown error during HTTP request: {webEx.Message}");
                            }
                        }
                    }
                }
                else
                {
                    Log.Debug($"{pluginInfo.Name} isn't installed on the server. The update will be ignored.");
                }
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

        internal static void CreateCustomPluginListFile()
        {
            try
            {
                // Check if the file already exists
                if (!File.Exists(Path.Combine(Exiled.API.Features.Paths.Configs, "Custom-Updater.yml")))
                {
                    // Create a custom plugin list example
                    var customPluginList = new List<PluginInfo>
                {
                    new PluginInfo { Name = "CustomPlugin1", GitHubRepoUrl = "https://github.com/user/repo1" },
                    new PluginInfo { Name = "CustomPlugin2", GitHubRepoUrl = "https://github.com/user/repo2" },
                    
                };

                    // Serialize the list of plugins
                    var serializer = new Serializer();
                    var yamlContent = serializer.Serialize(customPluginList);

                    // Write the YAML content to the file
                    File.WriteAllText(Path.Combine(Exiled.API.Features.Paths.Configs, "Custom-Updater.yml"), yamlContent, Encoding.UTF8);

                    Log.Warn($"I am creating Custom-Updater.yml file in the config folder....");
                }
                else
                {
                    Log.Debug($"Custom-Updater.yml file already exists in the config folder.");
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
