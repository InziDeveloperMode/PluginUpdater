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

namespace PluginUpdater
{
        public class EventsHandler
        {

        public static void UpdatePlugins()
        {
            foreach (var pluginInfo in Main.Instance.pluginsToUpdate)
            {
                // Check if the plugin is installed before attempting the update
                if (IsPluginInstalled(pluginInfo.Name))
                {
                    try
                    {
                        // Check if the update is available
                        if (IsUpdateAvailable(pluginInfo))
                        {
                            Log.Warn($"Checking the plugin {pluginInfo.Name}....");
                            UpdatePlugin(pluginInfo);
                            
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
                                Log.Error("Name resolution error: Make sure your server has a working network connection.\r\n");
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

        public static bool IsPluginInstalled(string pluginName)
        {
            // Check if the plugin DLL file exists in the plugins directory
            string pluginPath = Path.Combine(Paths.Plugins, $"{pluginName}.dll");
            return File.Exists(pluginPath);
        }

        public static bool IsUpdateAvailable(PluginInfo pluginInfo)
        {
            // It will be updated in the next version

            return true;
        }

        public static void UpdatePlugin(PluginInfo pluginInfo)
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


        public static void DownloadFile(string fileUrl, string savePath)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(fileUrl, savePath);
            }
        }


    }
}
